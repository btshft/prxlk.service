using System;
using MediatR;

namespace Prxlk.Application.Shared.Messages
{
    public abstract class Command : IRequest<Unit>
    {
        public DateTime Timestamp { get; }

        protected Command()
        {
            Timestamp = DateTime.UtcNow;
        }
    }
}