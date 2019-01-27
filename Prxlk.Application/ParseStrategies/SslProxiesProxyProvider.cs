using System;
using Prxlk.Contracts;

namespace Prxlk.Application.ParseStrategies
{
    public class SslProxiesProxyProvider : IExternalProxyProvider
    {
        /// <inheritdoc />
        public ProxyExternalResult GetProxies(int maxCount)
        {
            return new ProxyExternalResult(Array.Empty<ProxyTransportModel>(), ProxySource.SslProxies);
        }
    }
}