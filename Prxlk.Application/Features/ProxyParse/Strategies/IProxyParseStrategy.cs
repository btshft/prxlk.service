using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Prxlk.Contracts;

namespace Prxlk.Application.Features.ProxyParse.Strategies
{
    public interface IProxyParseStrategy
    {
        Task<IReadOnlyCollection<ProxyTransportModel>> ParseAsync(
            ProxyParseRequest request, CancellationToken cancellation);
    }
}