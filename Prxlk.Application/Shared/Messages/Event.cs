using System;
using MediatR;

namespace Prxlk.Application.Shared.Messages
{
    public abstract class Event : Message, INotification
    {
        protected Event(Guid correlationId) 
            : base(correlationId)
        { }

        protected Event()
        { }
    }
}