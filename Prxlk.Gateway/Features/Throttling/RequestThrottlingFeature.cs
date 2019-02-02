using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Prxlk.Gateway.Features.Throttling
{
    [GatewayFeature("RequestThrottling")]
    public class RequestThrottlingFeature : GatewayFeature
    {
        /// <inheritdoc />
        public override void RegisterFeature(IServiceCollection services)
        {
            // TODO
        }

        /// <inheritdoc />
        public override void UseFeature(IApplicationBuilder app)
        {
            // TODO
        }
    }
}