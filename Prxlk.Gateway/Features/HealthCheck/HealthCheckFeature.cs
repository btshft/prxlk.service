using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prxlk.Application.Shared.DependencyInjection;
using Prxlk.Data.MongoDb;
using Prxlk.Gateway.Features.ScheduledEventEmit;

namespace Prxlk.Gateway.Features.HealthCheck
{
    [GatewayFeature("HealthCheck", order: 2)]
    public class HealthCheckFeature : GatewayFeature
    {
        /// <inheritdoc />
        public override void RegisterFeature(IServiceCollection services, IConfiguration configuration)
        {
            var healthChecksBuilder = services.AddHealthChecks()
                .Add(new HealthCheckRegistration("mongo", sp =>
                        new MongoHealthCheck(sp.GetRequiredService<IMongoDatabaseProvider>()),
                    null, null));

            if (configuration.IsFeatureEnabled<ScheduledEventEmitFeature>())
            {
                healthChecksBuilder.Add(new HealthCheckRegistration("event_emitter", sp =>
                        new EventEmitterHealthCheck(sp.GetRequiredService<IScopedServiceFactory<IMediator>>()),
                    null, null));
            }
        }

        /// <inheritdoc />
        public override void UseFeature(IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = HealthResponseWriter
            });
        }

        private static Task HealthResponseWriter(HttpContext context, HealthReport report)
        {
            context.Response.ContentType = "application/json";

            var json = new JObject(
                new JProperty("status", report.Status.ToString()),
                new JProperty("results", new JObject(report.Entries.Select(pair =>
                    new JProperty(pair.Key, new JObject(
                        new JProperty("status", pair.Value.Status.ToString()),
                        new JProperty("description", pair.Value.Description),
                        new JProperty("data", new JObject(pair.Value.Data.Select(
                            p => new JProperty(p.Key, p.Value))))))))));

            return context.Response.WriteAsync(json.ToString(Formatting.Indented));
        }
    }
}