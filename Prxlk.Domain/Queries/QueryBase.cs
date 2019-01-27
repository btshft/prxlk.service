using System;
using MediatR;

namespace Prxlk.Domain.Queries
{
    /// <summary>
    /// Base proxy query class.
    /// </summary>
    public abstract class QueryBase<T> : IRequest<T> where T : class
    {
        /// <summary>
        /// Unique query identifier.
        /// </summary>
        public Guid QueryId { get; }

        /// <summary>
        /// Initializes new instance of <see cref="QueryBase{T}"/>.
        /// </summary>
        protected QueryBase()
        {
            QueryId = Guid.NewGuid();
        }

        public abstract bool Validate(out string[] errors);
    }
}