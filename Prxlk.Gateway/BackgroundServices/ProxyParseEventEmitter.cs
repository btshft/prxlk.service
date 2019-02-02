using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Prxlk.Application.Features.ProxyParse;
using Prxlk.Application.Shared.DependencyInjection;
using Prxlk.Application.Shared.Messages;
using Prxlk.Application.Shared.Options;
using Prxlk.Contracts;
using Prxlk.Gateway.Features.Diagnostics;

namespace Prxlk.Gateway.BackgroundServices
{
    public class ProxyParseEventEmitter : IHostedService, IDisposable
    {
        private readonly List<Timer> _proxyParseEmitters;
        private readonly ServiceOptions _options;
        private readonly IScopedServiceFactory<IMediator> _mediatorFactory;
        private readonly DiagnosticSource _diagnosticSource;
        
        public ProxyParseEventEmitter(
            IOptions<ServiceOptions> options, 
            IScopedServiceFactory<IMediator> mediatorFactory)
        {
            _options = options.Value;
            _mediatorFactory = mediatorFactory;
            _proxyParseEmitters = new List<Timer>();
            _diagnosticSource = new DiagnosticListener(EventEmitterDiagnostic.ListenerName);
        }
        
        /// <inheritdoc />
        public Task StartAsync(CancellationToken cancellation)
        {
            StartEmitters(cancellation);           
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task StopAsync(CancellationToken cancellation)
        {
            StopEmitters();            
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            foreach (var emitter in _proxyParseEmitters)
                emitter.Dispose();
        }

        private void StartEmitters(CancellationToken cancellation)
        {
            foreach (var proxySource in Enum.GetValues(typeof(ProxySource)).Cast<ProxySource>())
            {
                if (proxySource == ProxySource.Undefined)
                    continue;

                var sourceOptions = _options.GetSource(proxySource);
                var currentProxySource = proxySource;
                
                _proxyParseEmitters.Add(CreateTimer(async _ =>
                {
                    var @event = new ProxyParseRequested(currentProxySource);
                    
                    try
                    {
                        using (var scope = _mediatorFactory.CreateScope())
                        {
                            var mediator = scope.GetRequiredService();                        
                            await mediator.Publish(@event, cancellation);
                        }
                    }
                    catch (Exception e)
                    {
                        if (_diagnosticSource.IsEnabled(EventEmitterDiagnostic.ExceptionEventName))
                            _diagnosticSource.Write(EventEmitterDiagnostic.ExceptionEventName, new
                            {
                                @event = (Event)@event, 
                                exception = e
                            });
                    }

                }, sourceOptions.Refresh,  _options.EmitterWaitTime));
            }
        }

        private void StopEmitters()
        {
            foreach (var timer in _proxyParseEmitters)
            {
                timer.Change(Timeout.Infinite, 0);
            }
        }
        
        private static Timer CreateTimer(TimerCallback callback, TimeSpan period, TimeSpan delay)
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            // Don't capture the current ExecutionContext and its AsyncLocals onto the timer
            var restoreFlow = false;
            try
            {
                if (ExecutionContext.IsFlowSuppressed()) 
                    return new Timer(callback, null, delay, period);
                
                ExecutionContext.SuppressFlow();
                restoreFlow = true;

                return new Timer(callback, null, delay, period);
            }
            finally
            {
                // Restore the current ExecutionContext
                if (restoreFlow)
                {
                    ExecutionContext.RestoreFlow();
                }
            }
        } 
    }
}