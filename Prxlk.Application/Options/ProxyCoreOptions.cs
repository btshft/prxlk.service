using System;

namespace Prxlk.Application.Options
{
    public class ProxyCoreOptions
    {   
        public string ConnectionString { get; set; }
        
        public ProxyInterval[] Intervals { get; set; }
        
        public class ProxyInterval
        {
            public string Name { get; set; }
            public TimeSpan Interval { get; set; }
        }
    }
}