using System;

namespace Prxlk.Gateway.BackgroundServices
{
    public class EventEmitterStatistics
    {
        public bool IsRunning { get; }
        public int FailedEmitCount { get; }
        public int SuccessEmitCount { get; }
        public DateTime? LastEmit { get; }
        public int RunningEmitters { get; }
        
        public EventEmitterStatistics(bool isRunning, int failedEmitCount, int successEmitCount, DateTime? lastEmit, int runningEmitters)
        {
            IsRunning = isRunning;
            FailedEmitCount = failedEmitCount;
            SuccessEmitCount = successEmitCount;
            LastEmit = lastEmit;
            RunningEmitters = runningEmitters;
        }

    }
}