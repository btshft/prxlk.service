using Prxlk.Contracts;

namespace Prxlk.Application.ParseStrategies
{
    public interface IExternalProxyProviderFactory
    {
        IExternalProxyProvider GetProvider(ProxySource source);
    }
}