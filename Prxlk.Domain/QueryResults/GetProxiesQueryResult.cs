using System;
using System.Collections.Generic;
using System.Linq;
using Prxlk.Domain.Models;

namespace Prxlk.Domain.QueryResults
{
    public class GetProxiesQueryResult : BaseQueryResult
    {
        public GetProxiesQueryResult(Guid queryId, IEnumerable<Proxy> proxies) 
            : base(queryId)
        {
            Proxies = proxies.ToArray();
        }

        public GetProxiesQueryResult(Guid queryId, IEnumerable<string> validationErrors) 
            : base(queryId, validationErrors)
        {
            Proxies = Array.Empty<Proxy>();
        }
       
        public IReadOnlyCollection<Proxy> Proxies { get; }
    }
}