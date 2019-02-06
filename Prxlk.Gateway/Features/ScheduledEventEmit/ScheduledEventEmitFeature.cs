using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Prxlk.Gateway.Features.ScheduledEventEmit
{
    [GatewayFeature("ScheduledEventEmit", order: 4)]
    public class ScheduledEventEmitFeature : GatewayFeature
    {
        /// <inheritdoc />
        public override void RegisterFeature(IServiceCollection services, IConfiguration configuration)
        {
            // Bg services
            services.AddSingleton<ScheduledEmitterHostedService>();
            services.AddSingleton<IHostedService>(sp => sp.GetRequiredService<ScheduledEmitterHostedService>());
            services.AddScoped<IRequestHandler<ScheduledEmitterHostedServiceHealthQuery, ScheduledEmitterHostedServiceHealth>>(
                sp => sp.GetRequiredService<ScheduledEmitterHostedService>());
        }

        /// <inheritdoc />
        public override void UseFeature(IApplicationBuilder app, IConfiguration configuration)
        {
            // do nothing
        }
    }
}