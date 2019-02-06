using System;
using System.Collections.Generic;

namespace Prxlk.Gateway.Features.ScheduledEventEmit.Emitters
{
    public class ScheduledEmitterContext : Dictionary<string, object>
    {
        public ScheduledEmitterContext()
        {
        }

        public ScheduledEmitterContext(IEqualityComparer<string> comparer) 
            : base(comparer)
        {
        }
        
        public T Get<T>(string key) => (T)this[key];

        public void Update<T>(string key, Func<T, T> update)
        {
            var value = (T)this[key];
            this[key] = update(value);
        }
    }
}