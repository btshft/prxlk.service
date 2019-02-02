using System;

namespace Prxlk.Application.Shared.Messages
{
    public abstract class Message
    {
        public DateTime Timestamp { get; }
        public Guid CorrelationId { get; }

        protected Message(Guid correlationId)
        {
            CorrelationId = correlationId;
            Timestamp = DateTime.UtcNow;
        }

        protected Message()
        {
            CorrelationId = Guid.Empty;
            Timestamp = DateTime.UtcNow;
        }
    }
}