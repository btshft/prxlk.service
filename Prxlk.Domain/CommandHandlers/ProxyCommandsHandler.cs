using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Prxlk.Domain.Commands;
using Prxlk.Domain.DataAccess;
using Prxlk.Domain.Models;

namespace Prxlk.Domain.CommandHandlers
{
    public class ProxyCommandsHandler : AsyncRequestHandler<AddProxiesCommand>
    {
        private readonly IRepository<Proxy> _proxyRepository;
        private readonly ILogger _logger;
        
        public ProxyCommandsHandler(IRepository<Proxy> proxyRepository, ILogger<ProxyCommandsHandler> logger)
        {
            _proxyRepository = proxyRepository;
            _logger = logger;
        }

        /// <inheritdoc />
        protected override async Task Handle(AddProxiesCommand command, CancellationToken cancellationToken)
        {
            if (!command.Validate(out var errors))
                throw new Exception($"Cannot add proxy {string.Join(", ", errors)}");

            var proxy = new Proxy(command.Ip, command.Port, command.Protocol, command.Country);
            var proxyId = await _proxyRepository.AddAsync(proxy, cancellationToken);
            // Send proxy insertion event
        }
    }
}