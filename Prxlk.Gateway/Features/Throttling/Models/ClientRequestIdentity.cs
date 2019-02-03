namespace Prxlk.Gateway.Features.Throttling.Models
{
    public class ClientRequestIdentity
    {
        public string ClientIp { get; }
        public string RequestPath { get; }
        public string RequestVerb { get; }
        public string RequestRoute { get; }
        
        public ClientRequestIdentity(string clientIp, string requestPath, string requestVerb, string requestRoute)
        {
            ClientIp = clientIp;
            RequestPath = requestPath;
            RequestVerb = requestVerb;
            RequestRoute = requestRoute;
        }
    }
}