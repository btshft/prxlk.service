using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Prxlk.Application.Shared.Handlers;
using Prxlk.Application.Shared.Messages;

namespace Prxlk.Application.Features.ProxyParse
{
    public class ProxyParseCoordinator : ProcessCoordinator, 
        IProcessCoordinator<ProxyParseRequested>, 
        IProcessCoordinator<ProxyParsed>,
        IProcessCoordinator<ProxyInsertedEvent>,
        IProcessCoordinator<ProxyParseFailed>
    {
        public ProxyParseCoordinator(IMediator mediator) 
            : base(mediator)
        {
        }
        
        /// <inheritdoc />
        public async Task<Message[]> ProcessAsync(ProxyParseRequested @event, CancellationToken cancellation)
        {
            return new Message[]
            {
                new ProxyParseCommand(@event.CorrelationId, @event.Source)
            };
        }

        /// <inheritdoc />
        public Task<Message[]> ProcessAsync(ProxyParsed @event, CancellationToken cancellation)
        {
            return Task.FromResult(new Message[]
            {
                new ProxyInsertCommand(
                    @event.CorrelationId,
                    @event.ParsedProxy.Ip,
                    @event.ParsedProxy.Port,
                    @event.ParsedProxy.Protocol,
                    @event.ParsedProxy.Country),
            });
        }  
        
        /// <inheritdoc />
        public async Task<Message[]> ProcessAsync(ProxyInsertedEvent @event, CancellationToken cancellation)
        {
            // Thats all
            return Array.Empty<Message>();
        }

        /// <inheritdoc />
        public async Task<Message[]> ProcessAsync(ProxyParseFailed @event, CancellationToken cancellation)
        {
            // TODO Log
            return Array.Empty<Message>();
        }
    }
}