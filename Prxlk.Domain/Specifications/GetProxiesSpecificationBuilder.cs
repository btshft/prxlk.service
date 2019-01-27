using System;
using Prxlk.Domain.Models;
using Prxlk.Domain.Queries;
using Prxlk.Domain.Specifications.Shared;

namespace Prxlk.Domain.Specifications
{
    public class GetProxiesSpecificationBuilder
    {
        private GetProxiesSpecificationBuilder(){ }

        public static Specification<Proxy> FromQuery(GetProxiesQuery query)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));
            
            // TODO
            return Specification<Proxy>.True;
        }
    }
}