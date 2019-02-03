using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Prxlk.IdentityService.Features.Models;

namespace Prxlk.IdentityService.Controllers
{
    [Route("api/[controller]"), ApiController]
    public class IdentityController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetIdentity()
        {
            var context = ControllerContext.HttpContext;
            
            var identity = new RequestIdentity
            {
                ClientIp = context.Connection.RemoteIpAddress.ToString(),
                TraceId = context.TraceIdentifier,
                Headers = context.Request.Headers
                    .ToDictionary(
                        h => h.Key.ToString(), 
                        h => h.Value.ToString()),
                Path = context.Request.Path.ToString(),
                Method = context.Request.Method,
                Protocol = context.Request.Protocol,
                Cookies = context.Request.Cookies.ToDictionary(c => c.Key, c => c.Value)
            };
            
            var certificate = await context.Connection.GetClientCertificateAsync();
            if (certificate != null)
            {
                identity.Certificate = new RequestCertificate
                {
                   Thumbprint = certificate.Thumbprint,
                   Issuer = certificate.Issuer,
                   SerialNumber = certificate.Thumbprint,
                   Subject = certificate.Subject,
                   SignatureAlgorithm = certificate.SignatureAlgorithm.FriendlyName,
                   NotAfter = certificate.NotAfter,
                   NotBefore = certificate.NotBefore
                };
            }
            
            return Ok(identity);
        }
    }
}