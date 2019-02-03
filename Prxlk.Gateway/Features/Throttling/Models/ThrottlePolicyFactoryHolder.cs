using System;

namespace Prxlk.Gateway.Features.Throttling.Models
{
    public class ThrottlePolicyFactoryHolder
    {
        public string Name { get; }
        public Func<ThrottlePolicy> InternalFactory { get; }
        
        public ThrottlePolicyFactoryHolder(string name, Func<ThrottlePolicy> internalFactory)
        {
            Name = name;
            InternalFactory = internalFactory;
        }
    }
}