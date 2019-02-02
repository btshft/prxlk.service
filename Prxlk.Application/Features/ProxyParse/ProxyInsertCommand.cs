using System;
using Prxlk.Application.Shared.Messages;

namespace Prxlk.Application.Features.ProxyParse
{
    public class ProxyInsertCommand : Command
    {
        public string Ip { get; }
        public int Port { get; }
        public string Protocol { get; }
        public string Country { get; }
        
        public ProxyInsertCommand(Guid correlationId, string ip, int port, string protocol, string country)
            : base(correlationId)
        {
            Ip = ip;
            Port = port;
            Protocol = protocol;
            Country = country;
        }
    }
}