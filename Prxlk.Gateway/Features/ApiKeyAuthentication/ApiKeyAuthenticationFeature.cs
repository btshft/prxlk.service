using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Prxlk.Gateway.Features.ApiKeyAuthentication
{
    [GatewayFeature("ApiKeyAuthentication")]
    public class ApiKeyAuthenticationFeature : GatewayFeature
    {
        /// <inheritdoc />
        public override void RegisterFeature(IServiceCollection services)
        {
        }

        /// <inheritdoc />
        public override void UseFeature(IApplicationBuilder app)
        {
        }
    }
}