using System.Collections.Generic;
using System.Linq;

namespace Prxlk.Contracts
{
    public class ProxyEnvelope
    {
        public IReadOnlyCollection<ProxyTransportModel> Proxies { get; }
        
        public ProxyEnvelope(IEnumerable<ProxyTransportModel> proxies)
        {
            Proxies = proxies.ToArray();
        }
    }
}