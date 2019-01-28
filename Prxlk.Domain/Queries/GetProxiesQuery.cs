using System;
using System.Collections.Generic;
using Prxlk.Domain.QueryResults;

namespace Prxlk.Domain.Queries
{
    /// <summary>
    /// Query model to get proxies.
    /// </summary>
    public class GetProxiesQuery : QueryBase<GetProxiesQueryResult>
    {
        public int Count { get; }
        public int Offset { get; }
        
        public GetProxiesQuery(int count, int offset)
        {
            Count = count;
            Offset = offset;
        }
        
        /// <inheritdoc />
        public override bool Validate(out string[] errors)
        {      
            errors = Array.Empty<string>();
            return true;
        }
    }
}