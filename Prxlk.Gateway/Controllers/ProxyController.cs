using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Prxlk.Application.Services;
using Prxlk.Contracts;

namespace Prxlk.Gateway.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController, ApiVersion("1.0")]
    public class ProxyController : ControllerBase
    {
        private readonly IProxyService _proxyService;

        public ProxyController(IProxyService proxyService)
        {
            _proxyService = proxyService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ProxyEnvelope), statusCode: 200)]
        [Produces("application/json")]
        public async Task<IActionResult> GetProxies([FromQuery] ProxyRequest request)
        {
            var result = await _proxyService.GetProxiesAsync(request, CancellationToken.None)
                .ConfigureAwait(continueOnCapturedContext: false);
            
            return Ok(result);
        }
    }
}