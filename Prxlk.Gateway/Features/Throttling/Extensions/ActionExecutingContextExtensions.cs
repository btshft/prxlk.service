using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc.Filters;
using Prxlk.Gateway.Features.Throttling.Models;

namespace Prxlk.Gateway.Features.Throttling.Extensions
{
    public static class ActionExecutingContextExtensions
    {
        private static string RealIpHeader = "X-Real-IP";
        
        public static ClientRequestIdentity GetClientIdentity(this ActionExecutingContext actionContext)
        {
            var context = actionContext.HttpContext;   
            var clientIp = context.Connection.RemoteIpAddress;
            var route = actionContext.ActionDescriptor.AttributeRouteInfo;
            
            if (context.Request.Headers.TryGetValue(RealIpHeader, out var headers))
            {
                var lastParsedIp = IPAddress.None;
                if (headers.LastOrDefault(v => v != null && IPAddress.TryParse((string) v, out lastParsedIp)) != null)
                {
                    clientIp = lastParsedIp;
                }
            }

            return new ClientRequestIdentity(
                clientIp.ToString(), context.Request.Path.ToString().ToLowerInvariant(), 
                context.Request.Method.ToLowerInvariant(), route?.Template.ToLowerInvariant());
        }
    }
}