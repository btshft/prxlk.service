using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Prxlk.Gateway.Features.Throttling.Models;

namespace Prxlk.Gateway.Features.Throttling
{
    public class ThrottlePolicyBuilder
    {
        private readonly IServiceCollection _serviceCollection;
        private readonly List<string> _acquiredNames;

        public ThrottlePolicyBuilder(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
            _acquiredNames = new List<string>();
        }

        public ThrottlePolicyBuilder AddDefaultPolicy(TimeSpan period, int requestCount, IReadOnlyCollection<string> whiteListRules)
        {
            if (_acquiredNames.Contains(string.Empty))
                throw new Exception("Default proxy already configured");
            
            _serviceCollection.AddSingleton(
                new ThrottlePolicyFactoryHolder(
                    string.Empty,
                    () => new ThrottlePolicy(
                        string.Empty, requestCount, period, whiteListRules)));
            
            _acquiredNames.Add(string.Empty);
            return this;
        }
    }
}