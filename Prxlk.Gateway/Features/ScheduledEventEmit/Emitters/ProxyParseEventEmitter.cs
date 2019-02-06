using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Prxlk.Application.Features.ProxyParse;
using Prxlk.Application.Shared.DependencyInjection;
using Prxlk.Contracts;

namespace Prxlk.Gateway.Features.ScheduledEventEmit.Emitters
{
    [EventEmitterCategory("ProxyParse")]
    public class ProxyParseEventEmitter : ScheduledEmitter
    {
        private readonly IScopedServiceFactory<IMediator> _mediatorScope;
        private readonly ProxySource _proxySource;
        
        public ProxyParseEventEmitter(
            TimeSpan initialDelay, 
            TimeSpan interval, 
            CancellationToken cancellation, 
            DiagnosticSource diagnosticSource, 
            IScopedServiceFactory<IMediator> mediatorScope, 
            ProxySource proxySource) 
            : base(initialDelay, interval, cancellation, diagnosticSource)
        {
            _mediatorScope = mediatorScope;
            _proxySource = proxySource;
            
            Context["source"] = _proxySource.ToString();
        }

        /// <inheritdoc />
        public override string GetName()
        {
            return $"{typeof(ProxyParseEventEmitter).Name}_{_proxySource}";
        }

        /// <inheritdoc />
        protected override async Task ExecuteAsync(CancellationToken cancellation)
        {
            var @event = new ProxyParseRequested(_proxySource);
            
            using (var scope = _mediatorScope.CreateScope())
            {
                var mediator = scope.GetRequiredService();
                await mediator.Publish(@event, cancellation);
            }
        }
    }
}