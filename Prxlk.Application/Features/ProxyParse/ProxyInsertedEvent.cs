using System;
using Prxlk.Application.Shared.Messages;

namespace Prxlk.Application.Features.ProxyParse
{
    public class ProxyInsertedEvent : Event
    {
        public ProxyInsertedEvent(Guid correlationId) 
            : base(correlationId)
        {
        }
    }
}