using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Prxlk.IdentityService.Features.ExceptionHandling
{    
    [GatewayFeature("GlobalExceptionHandling", order: 0)]
    public class GlobalExceptionHandlingFeature : GatewayFeature
    {
        /// <inheritdoc />
        public override void RegisterFeature(IServiceCollection services, IConfiguration configuration)
        {
            // do nothing
        }

        /// <inheritdoc />
        public override void UseFeature(IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
        }
    }
}