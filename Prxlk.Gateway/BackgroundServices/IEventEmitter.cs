using System;
using System.Threading;
using MediatR;
using Prxlk.Application.Shared.Messages;

namespace Prxlk.Gateway.BackgroundServices
{
    public interface IEventEmitter: IRequestHandler<EventEmitterStatisticsRequest, EventEmitterStatistics>
    {
        Timer CreateEmitter<TEvent>(TimeSpan refresh, Func<TEvent> eventFactory, CancellationToken cancellation) 
            where TEvent : Event;
    }
}