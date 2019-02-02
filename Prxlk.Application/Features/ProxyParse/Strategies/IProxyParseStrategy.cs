using System.Threading;
using System.Threading.Tasks;
using Prxlk.Contracts;

namespace Prxlk.Application.Features.ProxyParse.Strategies
{
    public interface IProxyParseStrategy
    {
        Task<ProxyTransportModel> GetNextAsync(CancellationToken cancellation);
    }
}