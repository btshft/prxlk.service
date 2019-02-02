using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Prxlk.Gateway.Features
{
    public abstract class GatewayFeature
    {
        protected GatewayFeature() { }
        
        public abstract void RegisterFeature(IServiceCollection services);
        public abstract void UseFeature(IApplicationBuilder app);
    }
}