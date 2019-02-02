using System;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Prxlk.Gateway.Features.Diagnostics;
using Prxlk.Gateway.Models;

namespace Prxlk.Gateway.Features.ExceptionHandling
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly DiagnosticSource _diagnosticSource;
        private readonly IHostingEnvironment _environment;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next, IHostingEnvironment environment)
        {
            _next = next;
            _environment = environment;
            _diagnosticSource = new DiagnosticListener(GatewayDiagnostic.ListenerName);
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                if (_diagnosticSource.IsEnabled(GatewayDiagnostic.UnhandledException))
                    _diagnosticSource.Write(GatewayDiagnostic.UnhandledException,
                        new { context = context, exception = ex });
                
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Handle user error messages

            var message = $"Exception of type '{exception.GetType().Name}' occured: {exception.Message}";
            var description = exception.ToString();

            message = _environment.IsDevelopment() ? message : "Unexpected error";
            description = _environment.IsDevelopment() ? description : "Unexpected error occured";
            
            var apiResponse = new ApiError(message, description, context.TraceIdentifier);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            
            await context.Response.WriteAsync(
                JsonConvert.SerializeObject(apiResponse, Formatting.Indented), 
                    Encoding.UTF8);
        }
    }
}