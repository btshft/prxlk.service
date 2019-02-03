using System;
using System.Collections.Generic;

namespace Prxlk.Gateway.Features.Throttling
{
    public class ThrottlePolicy 
    {
        public string Name { get; }
        public int Limit { get; }
        public TimeSpan Period { get; }
        public IReadOnlyCollection<string> IpWhitelist { get; }
        
        public ThrottlePolicy(string name, int limit, TimeSpan period, IReadOnlyCollection<string> ipWhitelist)
        {
            Limit = limit;
            Period = period;
            Name = name;
            IpWhitelist = ipWhitelist ?? Array.Empty<string>();
        }
    }
}