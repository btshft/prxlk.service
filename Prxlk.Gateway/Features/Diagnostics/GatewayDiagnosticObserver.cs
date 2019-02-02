using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DiagnosticAdapter;
using Serilog;

namespace Prxlk.Gateway.Features.Diagnostics
{
    public class GatewayDiagnosticObserver : DiagnosticObserver
    {
        private readonly ILogger _logger;

        public GatewayDiagnosticObserver()
        {
            _logger = Log.ForContext<GatewayDiagnosticObserver>();
        }
        
        /// <inheritdoc />
        protected override bool IsMatch(string listenerName)
        {
            return listenerName == GatewayDiagnostic.ListenerName;
        }

        [DiagnosticName(GatewayDiagnostic.UnhandledException)]
        public void OnUnhandledException(HttpContext context, Exception exception)
        {
            var request = context.Request;
            
            _logger
                .ForContext("RequestHeaders", request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()), destructureObjects: true)
                .ForContext("RequestHost", request.Host)
                .ForContext("RequestProtocol", request.Protocol)
                .ForContext("TraceId", context.TraceIdentifier)
                .Error(exception, "HTTP {RequestMethod} {RequestPath}", 
                    context.Request.Method,
                    context.Request.Path);
        }
    }
}