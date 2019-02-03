using System;
using Microsoft.Extensions.Caching.Memory;
using Prxlk.Gateway.Features.Throttling.Models;

namespace Prxlk.Gateway.Features.Throttling.Store
{
    public class InMemoryThrottleCounterStore : IThrottleCounterStore
    {
        private readonly IMemoryCache _memoryCache;

        public InMemoryThrottleCounterStore(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        /// <inheritdoc />
        public bool TryGet(string key, out ThrottleCounter counter)
        {
            counter = null;
            if (_memoryCache.TryGetValue<ThrottleCounter>(key, out var cache))
            {
                counter = cache;
                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public void Set(string key, ThrottleCounter counter, TimeSpan absoluteExpiration)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            
            if (counter == null)
                throw new ArgumentNullException(nameof(counter));

            _memoryCache.Set(key, counter, new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(absoluteExpiration));
        }
    }
}