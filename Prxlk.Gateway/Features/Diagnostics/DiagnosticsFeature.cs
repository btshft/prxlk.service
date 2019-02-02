using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Prxlk.Gateway.Features.Diagnostics
{
    [GatewayFeature("Diagnostics")]
    public class DiagnosticsFeature : GatewayFeature
    {
        /// <inheritdoc />
        public override void RegisterFeature(IServiceCollection services)
        {
            services.AddDiagnosticObserver<EventEmitterDiagnosticObserver>();
        }

        /// <inheritdoc />
        public override void UseFeature(IApplicationBuilder app)
        {
            app.UseDiagnosticListeners();
        }
    }
}