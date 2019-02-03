using System;

namespace Prxlk.Gateway.Features.Throttling.Store
{
    public interface IThrottlePolicyStore
    {
        ThrottlePolicy GetRequired(string name);
        ThrottlePolicy GetOrAdd(string name, Func<string, ThrottlePolicy> factory);
        void Add(ThrottlePolicy policy);
    }
}