using System;
using MediatR;

namespace Prxlk.Application.Shared.Messages
{
    public abstract class Command : Message, IRequest<Unit>
    {
        protected Command(Guid correlationId) 
            : base(correlationId)
        { }

        protected Command()
        { }
    }
}