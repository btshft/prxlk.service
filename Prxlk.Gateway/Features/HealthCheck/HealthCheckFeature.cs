using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Prxlk.Gateway.Features.HealthCheck
{
    [GatewayFeature("HealthCheck")]
    public class HealthCheckFeature : GatewayFeature
    {
        /// <inheritdoc />
        public override void RegisterFeature(IServiceCollection services)
        {
            services.AddHealthChecks();
        }

        /// <inheritdoc />
        public override void UseFeature(IApplicationBuilder app)
        {
            app.UseHealthChecks("/health");
        }
    }
}