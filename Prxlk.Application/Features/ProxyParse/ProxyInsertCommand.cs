using Prxlk.Application.Shared.Messages;

namespace Prxlk.Application.Features.ProxyParse
{
    public class ProxyInsertCommand : Command
    {
        public string Ip { get; }
        public int Port { get; }
        public string Protocol { get; }
        public string Country { get; }
        
        public ProxyInsertCommand(string ip, int port, string protocol, string country)
        {
            Ip = ip;
            Port = port;
            Protocol = protocol;
            Country = country;
        }
    }
}