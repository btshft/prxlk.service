using System.Threading;
using System.Threading.Tasks;
using Prxlk.Contracts;

namespace Prxlk.Application.Services
{
    public interface IProxyService 
    {
        Task<ProxyEnvelope> GetProxiesAsync(ProxyRequest request, CancellationToken cancellation);
        Task AddProxyAsync(ProxyTransportModel proxy);
        Task<int> GetProxiesCount();
    }
}