using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Prxlk.Gateway.Features.Diagnostics
{
    [GatewayFeature("Diagnostics")]
    public class DiagnosticsFeature : GatewayFeature
    {
        /// <inheritdoc />
        public override void RegisterFeature(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDiagnosticObserver<EventEmitterDiagnosticObserver>();
        }

        /// <inheritdoc />
        public override void UseFeature(IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseDiagnosticListeners();
        }
    }
}