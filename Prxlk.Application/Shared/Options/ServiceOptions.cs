using System;
using System.Linq;
using Prxlk.Contracts;

namespace Prxlk.Application.Shared.Options
{
    public class ServiceOptions
    {
        public string MongoDbConnectionString { get; set; }
        public string MongoDbDatabaseName { get; set; }
        public ProxySourceOption[] Sources { get; set; }

        public ProxySourceOption GetSource(ProxySource source)
        {
            if (Sources == null || Sources.Length == 0)
                throw new InvalidOperationException("Sources is empty. Check configuration");

            var sourceOption = Sources.FirstOrDefault(
                s => string.Equals(s.Name, source.ToString(), StringComparison.InvariantCultureIgnoreCase));
            
            if (sourceOption == null)
                throw new InvalidOperationException($"Source with name {source}' not found");

            return sourceOption;
        }
    }
}