using MediatR;
using Prxlk.Application.Shared.Messages;

namespace Prxlk.Application.Shared.Handlers
{
    public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Unit> 
        where TCommand : Command, IRequest<Unit>
    { }
}