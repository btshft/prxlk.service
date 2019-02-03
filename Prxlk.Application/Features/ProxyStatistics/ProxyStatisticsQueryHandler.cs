using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Prxlk.Application.Shared.Handlers;
using Prxlk.Contracts;
using Prxlk.Domain.DataAccess;
using Prxlk.Domain.DataAccess.QueryFold;
using Prxlk.Domain.Models;

namespace Prxlk.Application.Features.ProxyStatistics
{
    public class ProxyStatisticsQueryHandler : IQueryHandler<GetProxyStatisticsQuery, ProxyStatisticsResult>
    {
        private readonly IQueryRepository<Proxy> _proxyRepository;

        public ProxyStatisticsQueryHandler(IQueryRepository<Proxy> proxyRepository)
        {
            _proxyRepository = proxyRepository;
        }

        /// <inheritdoc />
        public Task<ProxyStatisticsResult> Handle(GetProxyStatisticsQuery request, CancellationToken cancellationToken)
        {
            var fold = QueryFoldPipeline.Create<Proxy>()
                .Compose(s => new
                {
                    Count = s.Count()
                });

            var proxyStatistics = _proxyRepository.QueryFold(fold);
            
            return Task.FromResult(new ProxyStatisticsResult
            {
                TotalProxies = proxyStatistics.Count
            });
        }
    }
}