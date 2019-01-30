using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Prxlk.Application.Features.ProxyParse;
using Prxlk.Application.Features.ProxyReturn;
using Prxlk.Application.Shared.Handlers;
using Prxlk.Contracts;

namespace Prxlk.Gateway.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController, ApiVersion("1.0")]
    public class ProxyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProxyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ProxyEnvelope), statusCode: 200)]
        [Produces("application/json")]
        public async Task<IActionResult> GetProxies([FromQuery] GetProxyRequest request)
        {
            var query = new GetProxiesQuery(request.Count, request.Offset);
            var envelope = await _mediator.Send(query);
            
            return Ok(envelope);
        }
    }
}