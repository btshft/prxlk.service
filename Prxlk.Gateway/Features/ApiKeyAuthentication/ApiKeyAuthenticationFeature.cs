using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Prxlk.Gateway.Features.ApiKeyAuthentication
{
    [GatewayFeature("ApiKeyAuthentication", order: 2)]
    public class ApiKeyAuthenticationFeature : GatewayFeature
    {
        /// <inheritdoc />
        public override void RegisterFeature(IServiceCollection services, IConfiguration configuration)
        {
        }

        /// <inheritdoc />
        public override void UseFeature(IApplicationBuilder app, IConfiguration configuration)
        {
        }
    }
}