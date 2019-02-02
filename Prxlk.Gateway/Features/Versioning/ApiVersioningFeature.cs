using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Prxlk.Gateway.Features.Versioning
{
    [GatewayFeature("ApiVersioning", order: 2)]
    public class ApiVersioningFeature : GatewayFeature
    {
        /// <inheritdoc />
        public override void RegisterFeature(IServiceCollection services, IConfiguration configuration)
        {
            services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.ReportApiVersions = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
            });
        }

        /// <inheritdoc />
        public override void UseFeature(IApplicationBuilder app, IConfiguration configuration)
        {
            // do nothing
        }
    }
}