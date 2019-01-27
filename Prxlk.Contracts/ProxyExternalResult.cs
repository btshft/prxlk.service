using System.Collections.Generic;

namespace Prxlk.Contracts
{
    public class ProxyExternalResult
    {
        public ProxyExternalResult(IReadOnlyCollection<ProxyTransportModel> proxies, ProxySource source)
        {
            Proxies = proxies;
            Source = source;
        }

        public IReadOnlyCollection<ProxyTransportModel> Proxies { get; }
        
        public ProxySource Source { get; }
    }
}