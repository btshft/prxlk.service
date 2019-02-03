using System;
using System.Collections.Generic;
using System.Linq;
using Prxlk.Gateway.Features.Throttling.Models;

namespace Prxlk.Gateway.Features.Throttling
{
    public class ThrottlePolicyFactoryProvider
    {
        private readonly IEnumerable<ThrottlePolicyFactoryHolder> _factories;

        public ThrottlePolicyFactoryProvider(IEnumerable<ThrottlePolicyFactoryHolder> factories)
        {
            _factories = factories;
        }

        public Func<ThrottlePolicy> GetFactory(string name)
        {
            var factoryHolder = _factories.FirstOrDefault(n =>
                string.Equals(n.Name, name, StringComparison.InvariantCulture));
            
            if (factoryHolder == null)
                throw new Exception($"Throttle policy with name '{name}' is not registered");

            return factoryHolder.InternalFactory;
        }
    }
}