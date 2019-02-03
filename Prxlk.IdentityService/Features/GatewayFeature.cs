using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Prxlk.IdentityService.Features
{
    public abstract class GatewayFeature
    {
        protected GatewayFeature() { }
        
        public abstract void RegisterFeature(IServiceCollection services, IConfiguration configuration);
        public abstract void UseFeature(IApplicationBuilder app, IConfiguration configuration);
    }
}