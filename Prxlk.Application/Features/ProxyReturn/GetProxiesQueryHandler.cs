using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Prxlk.Application.Shared.Extensions;
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
            var proxies = await _proxyRepository.QueryAsync(
                CreateQueryTransform(request));

            return new ProxyEnvelope { Proxies = proxies };
        }

        /// <inheritdoc />
        public IQueryTransform<Proxy, ProxyTransportModel> CreateQueryTransform(GetProxiesQuery query)
        {
            var transform = QueryTransformPipeline<Proxy>.Create()
                .Filter(_ => true) // TODO
                .Take(query.Limit);

            if (query.Offset.HasValue)
                transform = transform.Skip(query.Offset.Value);

            var projection = _mapper.GetProjection<Proxy, ProxyTransportModel>();
            
            return transform.Project(projection); 
        }
    }
}