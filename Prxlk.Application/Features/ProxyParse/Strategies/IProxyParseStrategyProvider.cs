using Prxlk.Contracts;

namespace Prxlk.Application.Features.ProxyParse.Strategies
{
    public interface IProxyParseStrategyProvider
    {
        IProxyParseStrategy GetStrategy(ProxySource source);
    }
}