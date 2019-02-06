using System.Collections.Generic;

namespace Prxlk.Gateway.Features.ScheduledEventEmit
{
    public class ScheduledEmitterHostedServiceHealth
    {
        public bool IsRunning { get; }
        public IReadOnlyDictionary<string, IReadOnlyDictionary<string, object>> Emitters { get; }
        
        public ScheduledEmitterHostedServiceHealth(bool isRunning, IReadOnlyDictionary<string, IReadOnlyDictionary<string, object>> emitters)
        {
            IsRunning = isRunning;
            Emitters = emitters;
        }
    }
}