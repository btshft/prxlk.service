using MediatR;
using Prxlk.Application.Shared.Messages;

namespace Prxlk.Application.Shared.Handlers
{
    public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, TResult>
        where TQuery : Query<TResult>, IRequest<TResult>
    { }
}