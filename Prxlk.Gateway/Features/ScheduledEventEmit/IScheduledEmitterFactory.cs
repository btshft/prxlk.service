using System.Collections.Generic;
using System.Threading;
using Prxlk.Gateway.Features.ScheduledEventEmit.Emitters;

namespace Prxlk.Gateway.Features.ScheduledEventEmit
{
    public interface IScheduledEmitterFactory<out TEmitter> 
        where TEmitter : ScheduledEmitter
    {
        IEnumerable<TEmitter> Create(CancellationToken cancellation);
    }
}