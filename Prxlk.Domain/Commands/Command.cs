using System;
using MediatR;

namespace Prxlk.Domain.Commands
{
    public abstract class Command : IRequest
    {
        public DateTime Timestamp { get; }

        protected Command()
        {
            Timestamp = DateTime.UtcNow;
        }

        public abstract bool Validate(out string[] errors);
    }
}