using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Prxlk.Application.Shared.DependencyInjection;
using Prxlk.Application.Shared.Options;
using Prxlk.Contracts;
using Prxlk.Gateway.Features.Diagnostics;
using Prxlk.Gateway.Features.ScheduledEventEmit.Emitters;

namespace Prxlk.Gateway.Features.ScheduledEventEmit
{
    public class ScheduledEmitterHostedService : IHostedService,
        IScheduledEmitterFactory<ProxyParseEventEmitter>,
        IRequestHandler<ScheduledEmitterHostedServiceHealthQuery, ScheduledEmitterHostedServiceHealth>
    {
        private readonly List<ScheduledEmitter> _emitters;
        private readonly DiagnosticSource _diagnosticSource;
        private readonly ServiceOptions _options;
        private readonly IScopedServiceFactory<IMediator> _mediatorFactory;
        
        private bool _isRunning;

        public ScheduledEmitterHostedService(
            IOptions<ServiceOptions> options, 
            IScopedServiceFactory<IMediator> mediatorFactory)
        {
            _mediatorFactory = mediatorFactory;
            _emitters = new List<ScheduledEmitter>();
            _diagnosticSource = new DiagnosticListener(EventEmitterDiagnostic.ListenerName);
            _options = options.Value;
        }
        
        /// <inheritdoc />
        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _emitters.AddRange(Create(cancellationToken));
                _isRunning = true;
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
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _isRunning = false;
            
            var exceptions = new List<Exception>();
            foreach (var emitter in _emitters)
            {
                try
                {
                    emitter.Dispose();
                }
                catch (Exception e)
                {
                    exceptions.Add(e);
                }
            }
            
            _emitters.Clear();
            
            if (exceptions.Count > 0)
            {
                var exception = new AggregateException(exceptions);
                if (_diagnosticSource.IsEnabled(EventEmitterDiagnostic.FatalException))
                    _diagnosticSource.Write(EventEmitterDiagnostic.FatalException, new { exception });

                ExceptionDispatchInfo.Capture(exception).Throw();
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public IEnumerable<ProxyParseEventEmitter> Create(CancellationToken cancellation)
        {
            var emitters = new List<ProxyParseEventEmitter>();
            foreach (var proxySource in Enum.GetValues(typeof(ProxySource)).Cast<ProxySource>())
            {
                var currentSource = proxySource;
                if (currentSource == ProxySource.Undefined)
                    continue;

                var options = _options.GetSource(currentSource);
                var emitter = new ProxyParseEventEmitter(
                    options.Refresh,
                    _options.EmitterWaitTime,
                    cancellation,
                    _diagnosticSource, 
                    _mediatorFactory, 
                    proxySource);
                
                emitters.Add(emitter);
            }

            return emitters.ToArray();
        }
        
        /// <inheritdoc />
        public Task<ScheduledEmitterHostedServiceHealth> Handle(
            ScheduledEmitterHostedServiceHealthQuery request, CancellationToken cancellationToken)
        {
            var emitters = _emitters.ToDictionary(k => k.GetName(), k => k.GetContext());
           
            return Task.FromResult(
                new ScheduledEmitterHostedServiceHealth(_isRunning, emitters));
        }
    }
}