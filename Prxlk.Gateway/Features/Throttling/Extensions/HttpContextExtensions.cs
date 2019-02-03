using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;
using Prxlk.Gateway.Features.Throttling.Models;

namespace Prxlk.Gateway.Features.Throttling.Extensions
{
    public static class HttpContextExtensions
    {
        private static string RealIpHeader = "X-Real-IP";
        
        public static ClientRequestIdentity GetClientIdentity(this HttpContext context)
        {
            var clientIp = context.Connection.RemoteIpAddress;
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
                context.Request.Method.ToLowerInvariant());
        }
    }
}