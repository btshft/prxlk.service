using System.Collections.Generic;

namespace Prxlk.Gateway.Models
{
    public class RequestIdentity
    {
        public string ClientIp { get; set; }
        public IReadOnlyDictionary<string, string> Headers { get; set; } 
        public string TraceId { get; set; }
        public RequestCertificate Certificate { get; set; }
        public string Protocol { get; set; }
        public string Method { get; set; }
        public string Path { get; set; }
        public IReadOnlyDictionary<string, string> Cookies { get; set; }
    }
}