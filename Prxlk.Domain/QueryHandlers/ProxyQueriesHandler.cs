using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Prxlk.Domain.DataAccess;
using Prxlk.Domain.Models;
using Prxlk.Domain.Queries;
using Prxlk.Domain.QueryResults;
using Prxlk.Domain.Specifications;

namespace Prxlk.Domain.QueryHandlers
{
    public class ProxyQueriesHandler : IRequestHandler<GetProxiesQuery, GetProxiesQueryResult>
    {
        private readonly IQueryRepository<Proxy> _proxyRepository;

        public ProxyQueriesHandler(IQueryRepository<Proxy> proxyRepository)
        {
            _proxyRepository = proxyRepository;
        }

        /// <inheritdoc />
        public async Task<GetProxiesQueryResult> Handle(GetProxiesQuery query, CancellationToken cancellationToken)
        {
            if (!query.Validate(out var errors))
                return new GetProxiesQueryResult(query.QueryId, errors);

            var proxies = _proxyRepository.WhereAsync(
                GetProxiesSpecificationBuilder.FromQuery(query),
                limit: query.Count, offset: query.Offset);

            var proxyList = new List<Proxy>();
            
            await proxies.ForEachAsync(proxy => proxyList.Add(proxy), cancellationToken);
            
            return new GetProxiesQueryResult(query.QueryId, proxyList);
        }
    }
}