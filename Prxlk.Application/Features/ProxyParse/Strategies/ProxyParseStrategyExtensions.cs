using System.Reflection;
using Prxlk.Contracts;

namespace Prxlk.Application.Features.ProxyParse.Strategies
{
    public static class ProxyParseStrategyExtensions
    {
        public static ProxySource GetSource(this IProxyParseStrategy strategy)
        {
            return strategy.GetType().GetCustomAttribute<ProxyParseStrategyAttribute>().Source;
        }
    }
}