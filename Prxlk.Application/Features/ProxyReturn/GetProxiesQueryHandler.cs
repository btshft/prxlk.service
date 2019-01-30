using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Prxlk.Application.Shared.Handlers;
using Prxlk.Contracts;
using Prxlk.Domain.DataAccess;
using Prxlk.Domain.DataAccess.QueryTransform;
using Prxlk.Domain.Models;

namespace Prxlk.Application.Features.ProxyReturn
{ 
    public class GetProxiesQueryHandler : 
        IQueryHandler<GetProxiesQuery, ProxyEnvelope>, 
        IQueryTransformBuilder<Proxy, ProxyTransportModel, GetProxiesQuery>
    {
        private readonly IQueryRepository<Proxy> _proxyRepository;
        private readonly IMapper _mapper;
        
        public GetProxiesQueryHandler(IQueryRepository<Proxy> proxyRepository, IMapper mapper)
        {
            _proxyRepository = proxyRepository;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<ProxyEnvelope> Handle(GetProxiesQuery request, CancellationToken cancellationToken)
        {
            var proxies = await _proxyRepository.QueryTransformAsync(
                CreateQueryTransform(request));

            return new ProxyEnvelope { Proxies = proxies };
        }

        /// <inheritdoc />
        public QueryTransform<Proxy, ProxyTransportModel> CreateQueryTransform(GetProxiesQuery query)
        {
            var transform = QueryTransformPipeline<Proxy>.Create()
                .Filter(_ => true) // TODO
                .Take(query.Limit);

            if (query.Offset.HasValue)
                transform = transform.Skip(query.Offset.Value);

            return transform.Project(p => _mapper.Map<ProxyTransportModel>(p)); 
        }
    }
}