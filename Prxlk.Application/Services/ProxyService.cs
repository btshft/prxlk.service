using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Prxlk.Contracts;
using Prxlk.Domain.Commands;
using Prxlk.Domain.DataAccess;
using Prxlk.Domain.Models;
using Prxlk.Domain.Queries;

namespace Prxlk.Application.Services
{
    public class ProxyService : IProxyService
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IQueryRepository<Proxy> _queryRepository;

        public ProxyService(IMapper mapper, IMediator mediator, IQueryRepository<Proxy> queryRepository)
        {
            _mapper = mapper;
            _mediator = mediator;
            _queryRepository = queryRepository;
        }

        /// <inheritdoc />
        public async Task<ProxyEnvelope> GetProxiesAsync(ProxyRequest request, CancellationToken cancellation)
        {
            var query = _mapper.Map<GetProxiesQuery>(request);
            var result = await _mediator.Send(query, cancellation);

            if (result.HasErrors)
                throw new Exception(string.Join(", ", result.ValidationErrors));
            
            return new ProxyEnvelope(result.Proxies.Select(_mapper.Map<ProxyTransportModel>));
        }

        /// <inheritdoc />
        public async Task AddProxyAsync(ProxyTransportModel proxy)
        {
            var command = _mapper.Map<AddProxiesCommand>(proxy);
            await _mediator.Send(command)
                .ConfigureAwait(continueOnCapturedContext: false);
        }

        /// <inheritdoc />
        public Task<int> GetProxiesCount()
        {
            return _queryRepository.CountAsync();
        }
    }
}