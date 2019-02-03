using System;

namespace Prxlk.IdentityService.Features.Models
{
    public class RequestCertificate
    {
        public string SerialNumber { get; set; }
        public string Issuer { get; set; }
        public string Thumbprint { get; set; }
        public string Subject { get; set; }
        public string SignatureAlgorithm { get; set; }
        public DateTime NotBefore { get; set; }
        public DateTime NotAfter { get; set; }
    }
}