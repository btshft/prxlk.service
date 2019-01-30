using System;
using System.Linq.Expressions;
using MediatR;

namespace Prxlk.Application.Shared.Messages
{
    public abstract class Query<TResult> : IRequest<TResult>
    {
        public DateTime Timestamp { get; }

        protected Query()
        {
            Timestamp = DateTime.UtcNow;
        }
    }
}