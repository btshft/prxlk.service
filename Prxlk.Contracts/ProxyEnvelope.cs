using System;
using System.Collections.Generic;

namespace Prxlk.Contracts
{
    public class ProxyEnvelope
    {
        public IReadOnlyCollection<ProxyTransportModel> Proxies { get; set; }
        
        public ProxyEnvelope()
        {
            Proxies = Array.Empty<ProxyTransportModel>();
        }
    }
}