using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Prxlk.Gateway.Features.EventEmit
{
    [GatewayFeature("EventEmit", order: 4)]
    public class EventEmitFeature : GatewayFeature
    {
        /// <inheritdoc />
        public override void RegisterFeature(IServiceCollection services, IConfiguration configuration)
        {
            // Bg services
            services.AddSingleton<EventEmitService>();
            services.AddSingleton<IHostedService>(sp => sp.GetRequiredService<EventEmitService>());
            services.AddScoped<IRequestHandler<EventEmitterStatisticsRequest, EventEmitterStatistics>>(
                sp => sp.GetRequiredService<EventEmitService>());
        }

        /// <inheritdoc />
        public override void UseFeature(IApplicationBuilder app, IConfiguration configuration)
        {
            // do nothing
        }
    }
}