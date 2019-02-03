using System;
using Prxlk.Gateway.Features.Throttling.Models;

namespace Prxlk.Gateway.Features.Throttling.Store
{
    public interface IThrottleCounterStore
    {
        bool TryGet(string key, out ThrottleCounter counter);
        void Set(string key, ThrottleCounter counter, TimeSpan absoluteExpiration);
    }
}