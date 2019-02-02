using MediatR;

namespace Prxlk.Application.Shared.Messages
{
    public abstract class Query<TResult> : Message, IRequest<TResult>
    {
    }
}