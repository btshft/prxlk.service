using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Prxlk.Application.Shared.Handlers;
using Prxlk.Domain.DataAccess;
using Prxlk.Domain.DataAccess.QueryFold;
using Prxlk.Domain.Models;

namespace Prxlk.Application.Features.ProxyParse
{
    public class ProxyInsertCommandHandler : ICommandHandler<ProxyInsertCommand>
    {
        private readonly IRepository<Proxy> _proxyRepository;
        private readonly IMapper _mapper;

        public ProxyInsertCommandHandler(IRepository<Proxy> proxyRepository, IMapper mapper)
        {
            _proxyRepository = proxyRepository;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<Unit> Handle(ProxyInsertCommand request, CancellationToken cancellationToken)
        {
            var fold = QueryFoldPipeline.Create<Proxy>()
                .Compose(source => source
                    .FirstOrDefault(p => p.Ip == request.Ip && p.Port == request.Port));
                    
            var existingProxy = _proxyRepository.QueryFold(fold);
            if (existingProxy != null)
                return Unit.Value;
            
            var proxy = _mapper.Map<Proxy>(request);
            
            await _proxyRepository.AddAsync(proxy, cancellationToken);
            
            // Event can be sent there
            return Unit.Value;
        }
    }
}