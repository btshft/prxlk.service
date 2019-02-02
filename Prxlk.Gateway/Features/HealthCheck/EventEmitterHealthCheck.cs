using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Prxlk.Application.Shared.DependencyInjection;
using Prxlk.Gateway.BackgroundServices;

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
                    var statistics = await mediator.Send(new EventEmitterStatisticsRequest(), cancellationToken);
                    
                    if (statistics == null)
                        return HealthCheckResult.Unhealthy("Unable to get emitter statistics");

                    if (!statistics.IsRunning)
                        return HealthCheckResult.Unhealthy("Emitter is not running");
                    
                    var parameters = new Dictionary<string, object>();
                    if (statistics.LastEmit.HasValue)
                        parameters.Add("last_emit", statistics.LastEmit.Value.ToString("O"));
                    
                    parameters.Add("failed_emits", statistics.FailedEmitCount.ToString());
                    parameters.Add("success_emits", statistics.SuccessEmitCount.ToString());
                    parameters.Add("running_emitters", statistics.RunningEmitters.ToString());
                    
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