using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Prxlk.IdentityService.Features.Models;

namespace Prxlk.IdentityService.Features.ExceptionHandling
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostingEnvironment _environment;
        private readonly ILogger _logger;

        public GlobalExceptionHandlingMiddleware(
            RequestDelegate next,
            IHostingEnvironment environment,
            ILogger<GlobalExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _environment = environment;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Trace: {context.TraceIdentifier}");
                
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