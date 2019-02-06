using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json.Linq;
using Prxlk.Application.Shared.DependencyInjection;
using Prxlk.Gateway.Features.ScheduledEventEmit;

namespace Prxlk.Gateway.Features.HealthCheck
{
    public class EventEmitterHealthCheck : IHealthCheck
    {
        private readonly IScopedServiceFactory<IMediator> _mediatorScope;

        public EventEmitterHealthCheck(IScopedServiceFactory<IMediator> mediatorScope)
        {
            _mediatorScope = mediatorScope;
        }

        /// <inheritdoc />
        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                using (var scope = _mediatorScope.CreateScope())
                {
                    var mediator = scope.GetRequiredService();
                    var statistics = await mediator.Send(new ScheduledEmitterHostedServiceHealthQuery(), cancellationToken);
                    
                    if (statistics == null)
                        return HealthCheckResult.Unhealthy("Unable to get emitter statistics");

                    if (!statistics.IsRunning)
                        return HealthCheckResult.Unhealthy("Emitter service is not running");
                    
                    var parameters = new Dictionary<string, object>();
                    foreach (var (key, value) in statistics.Emitters)
                    {
                        parameters.Add(key, JObject.FromObject(value));
                    }
                    
                    return HealthCheckResult.Healthy(data: parameters);
                }
            }
            catch (Exception e)
            {
                return new HealthCheckResult(context.Registration.FailureStatus, exception: e);
            }
        }
    }
}