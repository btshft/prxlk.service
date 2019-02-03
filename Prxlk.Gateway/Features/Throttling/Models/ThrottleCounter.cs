using System;

namespace Prxlk.Gateway.Features.Throttling.Models
{
    public class ThrottleCounter
    {
        public DateTime Timestamp { get; }
        public int RequestCount { get; }
        
        public ThrottleCounter(DateTime timestamp, int requestCount)
        {
            Timestamp = timestamp;
            RequestCount = requestCount;
        }
        
        public ThrottleCounter(int requestCount)
        {
            Timestamp = DateTime.UtcNow;
            RequestCount = requestCount;
        }
    }
}