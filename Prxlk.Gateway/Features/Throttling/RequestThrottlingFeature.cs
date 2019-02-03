using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prxlk.Gateway.Features.Throttling.Extensions;

namespace Prxlk.Gateway.Features.Throttling
{
    [GatewayFeature("RequestThrottling", order: 2)]
    public class RequestThrottlingFeature : GatewayFeature
    {
        /// <inheritdoc />
        public override void RegisterFeature(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();
            services.AddRequestThrottling(builder =>
            {
                var whitelist = configuration.GetSection("Throttling:Whitelist")
                    .Get<string[]>();

                var limit = configuration.GetSection("Throttling:Limit")
                    .Get<int>();

                var period = configuration.GetSection("Throttling:Period")
                    .Get<TimeSpan>();
                
                builder.AddDefaultPolicy(period, limit, whitelist);
            });
        }

        /// <inheritdoc />
        public override void UseFeature(IApplicationBuilder app, IConfiguration configuration)
        {
            // TODO
        }
    }
}