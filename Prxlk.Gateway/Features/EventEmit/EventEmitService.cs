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
using Prxlk.Shared.Expressions;

namespace Prxlk.Gateway.Features.EventEmit
{
    public class EventEmitService : IHostedService, IEventEmitter
    {
        private readonly List<Timer> _runningEmitters;
        private readonly IScopedServiceFactory<IMediator> _mediatorScope;
        private readonly ServiceOptions _options;
        private readonly DiagnosticSource _diagnosticSource;

        private DateTime? _lastEmitTime;
        private int _successEmits, _failedEmits;
        private bool _isRunning;
        
        public EventEmitService(
            IScopedServiceFactory<IMediator> mediatorScope,
            IOptions<ServiceOptions> options)
        {
            _mediatorScope = mediatorScope;
            _runningEmitters = new List<Timer>();
            _options = options.Value;
            _diagnosticSource = new DiagnosticListener(EventEmitterDiagnostic.ListenerName);
        }
        
        /// <inheritdoc />
        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                foreach (var proxySource in Enum.GetValues(typeof(ProxySource)).Cast<ProxySource>())
                {
                    var currentSource = proxySource;
                    if (currentSource == ProxySource.Undefined)
                        continue;

                    var options = _options.GetSource(currentSource);
                    var emitter = CreateEmitter(
                        options.Refresh, () => new ProxyParseRequested(currentSource),
                        cancellationToken);

                    _runningEmitters.Add(emitter);
                }
            }
            catch (Exception e)
            {
                if (_diagnosticSource.IsEnabled(EventEmitterDiagnostic.FatalException))
                    _diagnosticSource.Write(EventEmitterDiagnostic.FatalException, new { exception = e });

                throw;
            }

            _isRunning = true;
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                _isRunning = false;
                
                foreach (var emitter in _runningEmitters)
                    emitter.Dispose();
                
                _runningEmitters.Clear();
            }
            catch (Exception e)
            {
                if (_diagnosticSource.IsEnabled(EventEmitterDiagnostic.FatalException))
                    _diagnosticSource.Write(EventEmitterDiagnostic.FatalException, new { exception = e });

                throw;
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task<EventEmitterStatistics> Handle(
            EventEmitterStatisticsRequest request, 
            CancellationToken cancellationToken)
        {
            var statistics = new EventEmitterStatistics(
                _isRunning, _failedEmits, _successEmits, _lastEmitTime, _runningEmitters.Count);

            return Task.FromResult(statistics);
        }

        /// <inheritdoc />
        public Timer CreateEmitter<TEvent>(TimeSpan refresh, Func<TEvent> eventFactory, CancellationToken cancellation) 
            where TEvent : Event
        {
            return TimerFactory.CreateTimer(async _ =>
            {
                TEvent @event = null;

                try
                {
                    @event = eventFactory();

                    using (var scope = _mediatorScope.CreateScope())
                    {
                        var mediator = scope.GetRequiredService();
                        await mediator.Publish(@event, cancellation);

                        _successEmits++;
                    }

                }
                catch (Exception e)
                {
                    if (_diagnosticSource.IsEnabled(EventEmitterDiagnostic.EmitExceptionEventName))
                        _diagnosticSource.Write(EventEmitterDiagnostic.EmitExceptionEventName, new
                        {
                            @event = (Event) @event,
                            exception = e
                        });

                    _failedEmits++;
                }
                finally
                {
                    _lastEmitTime = DateTime.UtcNow;
                }              
            }, refresh, _options.EmitterWaitTime);
        }
    }
}