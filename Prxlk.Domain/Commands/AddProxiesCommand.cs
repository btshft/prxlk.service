using System;

namespace Prxlk.Domain.Commands
{
    public class AddProxiesCommand : Command
    {
        public string Ip { get; }
        public int Port { get; }
        public string Protocol { get; }
        public string Country { get; }
        
        public AddProxiesCommand(string ip, int port, string protocol, string country)
        {
            Ip = ip;
            Port = port;
            Protocol = protocol;
            Country = country;
        }
        
        /// <inheritdoc />
        public override bool Validate(out string[] errors)
        {
            errors = Array.Empty<string>();
            return true;
        }
    }
}