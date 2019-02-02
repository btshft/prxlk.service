using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Prxlk.Gateway.Features.Versioning
{
    [GatewayFeature("ApiVersioning")]
    public class ApiVersioningFeature : GatewayFeature
    {
        /// <inheritdoc />
        public override void RegisterFeature(IServiceCollection services)
        {
            services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
            });
        }

        /// <inheritdoc />
        public override void UseFeature(IApplicationBuilder app)
        {
            // do nothing
        }
    }
}