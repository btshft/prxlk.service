using System;

namespace Prxlk.Application.Shared.Options
{
    public class ProxyCoreOptions
    {   
        public string MssqlConnectionString { get; set; }
        
        public string MongoDbConnectionString { get; set; }
        public string MongoDbDatabaseName { get; set; }
        
        public ProxyInterval[] Intervals { get; set; }
        
        public class ProxyInterval
        {
            public string Name { get; set; }
            public TimeSpan Interval { get; set; }
        }
    }
}