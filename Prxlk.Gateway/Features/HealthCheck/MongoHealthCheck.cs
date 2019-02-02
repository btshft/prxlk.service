using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Prxlk.Data.MongoDb;

namespace Prxlk.Gateway.Features.HealthCheck
{
    public class MongoHealthCheck : IHealthCheck
    {
        private readonly IMongoDatabaseProvider _databaseProvider;

        public MongoHealthCheck(IMongoDatabaseProvider databaseProvider)
        {
            _databaseProvider = databaseProvider;
        }

        /// <inheritdoc />
        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                var database = _databaseProvider.GetDatabase();
                await database.ListCollectionsAsync(
                    cancellationToken: cancellationToken);

                return HealthCheckResult.Healthy();
            }
            catch (Exception ex)
            {
                return new HealthCheckResult(context.Registration.FailureStatus, exception: ex);
            }
        }
    }
}