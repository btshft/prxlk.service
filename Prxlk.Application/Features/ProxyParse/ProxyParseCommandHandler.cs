using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Prxlk.Application.Features.ProxyParse.Strategies;
using Prxlk.Application.Shared.Handlers;

namespace Prxlk.Application.Features.ProxyParse
{
    public class ProxyParseCommandHandler : ICommandHandler<ProxyParseCommand>
    {
        private readonly IProxyParseStrategyProvider _strategyProvider;
        private readonly IMediator _mediator;

        public ProxyParseCommandHandler(
            IProxyParseStrategyProvider strategyProvider, 
            IMediator mediator)
        {
            _strategyProvider = strategyProvider;
            _mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<Unit> Handle(ProxyParseCommand request, CancellationToken cancellationToken)
        {
            var strategy = _strategyProvider.GetStrategy(request.Source);
            var proxy = await strategy.GetNextAsync(cancellationToken);

            do
            {
                await _mediator.Publish(new ProxyParsed(request.CorrelationId, proxy), cancellationToken);
                proxy = await strategy.GetNextAsync(cancellationToken);
                
            } while (proxy != null);
            
            return Unit.Value;
        }
    }
}