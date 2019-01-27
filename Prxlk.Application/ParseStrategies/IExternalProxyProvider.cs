using Prxlk.Contracts;

namespace Prxlk.Application.ParseStrategies
{
    public interface IExternalProxyProvider
    {
        ProxyExternalResult GetProxies(int maxCount);
    }
}