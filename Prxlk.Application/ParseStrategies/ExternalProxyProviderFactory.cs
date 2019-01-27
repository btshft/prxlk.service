using System;
using Prxlk.Contracts;

namespace Prxlk.Application.ParseStrategies
{
    public class ExternalProxyProviderFactory : IExternalProxyProviderFactory
    {
        public IExternalProxyProvider GetProvider(ProxySource source)
        {
            switch (source)
            {
                case ProxySource.SslProxies:
                    return new SslProxiesProxyProvider();
                default:
                    throw new ArgumentOutOfRangeException(nameof(source), source, null);
            }
        }
    }
}