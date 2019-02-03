using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc.Filters;
using Prxlk.Gateway.Features.Throttling.Extensions;
using Prxlk.Gateway.Features.Throttling.Models;
using Prxlk.Gateway.Features.Throttling.Store;

namespace Prxlk.Gateway.Features.Throttling
{
    public class ThrottlePolicyEvaluator
    {
        private static readonly object Sync = new object();
        
        private readonly IThrottleCounterStore _counterStore; 
        private ActionExecutingContext _context;

        public ThrottlePolicyEvaluator ForContext(ActionExecutingContext context)
        {
            _context = context;
            return this;
        }
        
        public ThrottlePolicyEvaluator(IThrottleCounterStore counterStore)
        {
            _counterStore = counterStore;
        }
        
        public ThrottlePolicyEvaluationResult Evaluate(ThrottlePolicy policy)
        {
            var identity = _context.GetClientIdentity();         
            if (policy.IsWhitelisted(identity.ClientIp))
                return ThrottlePolicyEvaluationResult.Bypass;
            
            var counter = new ThrottleCounter(requestCount: 1);
            var counterKey = GetCounterKey();

            lock (Sync)
            {
                // Counter already exists
                if (_counterStore.TryGet(counterKey, out var existing))
                {
                    var isCounterExpired = (existing.Timestamp + policy.Period) < DateTime.UtcNow;
                    if (!isCounterExpired)
                    {
                        // Update existing
                        var totalRequests = existing.RequestCount + 1;
                        counter = new ThrottleCounter(timestamp: existing.Timestamp, requestCount: totalRequests);
                    }
                }
                
                _counterStore.Set(counterKey, counter, policy.Period);
            }

            var reset = counter.Timestamp + policy.Period;
            var limit = policy.Period;
            var remaining = policy.Limit - counter.RequestCount + 1; // +1 for current
            
            return new ThrottlePolicyEvaluationResult(reset, remaining, limit);
            
            string GetCounterKey()
            {
                var key = $"request-throttling-meta:{identity.ClientIp}_{policy.Name}_{identity.RequestRoute}_{identity.RequestVerb}";
                var keyBytes = Encoding.UTF8.GetBytes(key);

                using (var sha1 = SHA1.Create())
                {
                    var hashedBytes = sha1.ComputeHash(keyBytes);
                    return BitConverter.ToString(hashedBytes)
                        .Replace("-", "");
                }
            }
        }
    }
}