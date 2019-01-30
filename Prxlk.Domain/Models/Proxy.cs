using System;

namespace Prxlk.Domain.Models
{
    public class Proxy : Entity<Guid>
    {
        public string Ip { get; protected set; }
        public int Port { get; protected set; }
        public string Protocol { get; protected set;  }
        public string Country { get; protected set; }
        
        protected Proxy() { }
        
        public Proxy(string ip, int port, string protocol, string country)
        {
            Id = Guid.NewGuid();
            Ip = ip;
            Port = port;
            Protocol = protocol;
            Country = country;
        }
    }
}