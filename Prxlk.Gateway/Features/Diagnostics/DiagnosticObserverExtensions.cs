using System.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Prxlk.Gateway.Features.Diagnostics
{
    public static class DiagnosticObserverExtensions
    {
        public static IServiceCollection AddDiagnosticObserver<TDiagnosticObserver>(
            this IServiceCollection services)
            where TDiagnosticObserver : DiagnosticObserver
        {
            services.TryAddEnumerable(ServiceDescriptor
                .Transient<DiagnosticObserver, TDiagnosticObserver>());

            return services;
        }

        public static IApplicationBuilder UseDiagnosticListeners(this IApplicationBuilder app)
        {
            var diagnosticObservers = app.ApplicationServices.GetServices<DiagnosticObserver>();
            foreach (var diagnosticObserver in diagnosticObservers)
            {
                DiagnosticListener.AllListeners.Subscribe(diagnosticObserver);
            }

            return app;
        }
    }
}