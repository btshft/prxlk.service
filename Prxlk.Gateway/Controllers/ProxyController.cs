using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Prxlk.Application.Features.ProxyReturn;
using Prxlk.Application.Features.ProxyStatistics;
using Prxlk.Contracts;
using Prxlk.Gateway.Features.Throttling;
using Prxlk.Gateway.Models;

namespace Prxlk.Gateway.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ProducesErrorResponseType(typeof(ApiError))]
    [ApiController, ApiVersion("1.0")]
    public class ProxyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProxyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet, Throttle]
        [ProducesResponseType(typeof(ProxyEnvelope), statusCode: 200)]
        [Produces("application/json")]
        public async Task<IActionResult> GetProxies([FromQuery] GetProxyRequest request)
        {
            var query = new GetProxiesQuery(request.Count, request.Offset);
            var envelope = await _mediator.Send(query);

            return Ok(envelope);
        }

        [HttpGet("statistics"), Throttle]
        [ProducesResponseType(typeof(ProxyStatisticsResult), statusCode: 200)]
        [Produces("application/json")]
        public async Task<IActionResult> GetStatistics()
        {
            var query = new GetProxyStatisticsQuery();
            var statistics = await _mediator.Send(query);

            return Ok(statistics);
        }
    }
}