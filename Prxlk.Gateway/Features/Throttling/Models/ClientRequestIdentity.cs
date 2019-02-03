namespace Prxlk.Gateway.Features.Throttling.Models
{
    public class ClientRequestIdentity
    {
        public string ClientIp { get; }
        public string RequestPath { get; }
        public string RequestVerb { get; }
        
        public ClientRequestIdentity(string clientIp, string requestPath, string requestVerb)
        {
            ClientIp = clientIp;
            RequestPath = requestPath;
            RequestVerb = requestVerb;
        }
    }
}