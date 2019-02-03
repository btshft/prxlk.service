using System;
using System.Collections.Generic;

namespace Prxlk.Gateway.Features.Throttling.Store
{
    public class InMemoryThrottlePolicyStore : IThrottlePolicyStore
    {
        private readonly Dictionary<string, ThrottlePolicy> _policyStore;
        private readonly object _sync;

        public InMemoryThrottlePolicyStore()
        {
            _policyStore = new Dictionary<string, ThrottlePolicy>(StringComparer.InvariantCultureIgnoreCase);
            _sync = new object();
        }

        /// <inheritdoc />
        public ThrottlePolicy GetRequired(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            
            lock (_sync)
            {
                if (_policyStore.TryGetValue(name, out var policy))
                    return policy;
                
                throw new Exception($"No throttling policy registered for name '{name}'");
            }
        }

        /// <inheritdoc />
        public ThrottlePolicy GetOrAdd(string name, Func<string, ThrottlePolicy> factory)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            lock (_sync)
            {
                if (_policyStore.TryGetValue(name, out var policy))
                    return policy;

                policy = factory(name);
                if (policy == null)
                    throw new Exception($"Unable to add throttle policy '{name}' - factory returned null");
                
                _policyStore.Add(name, policy);
                return policy;
            }
        }

        /// <inheritdoc />
        public void Add(ThrottlePolicy policy)
        {
            if (policy == null || policy.Name == null)
                throw new ArgumentNullException(nameof(policy));
            
            lock (_sync)
            {
                if (_policyStore.TryGetValue(policy.Name, out var _))
                    throw new Exception($"Throttling policy with name '{policy.Name}' already registered");
            }
        }
    }
}