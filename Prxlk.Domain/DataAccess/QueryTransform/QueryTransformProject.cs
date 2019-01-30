using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Prxlk.Domain.DataAccess.QueryTransform
{
    internal class QueryTransformProject<TIn, TOut> : IQueryTransform<TIn, TOut> 
        where TIn : class where TOut : class
    {
        private readonly Expression<Func<TIn, TOut>> _projection;

        public QueryTransformProject(Expression<Func<TIn, TOut>> projection)
        {
            _projection = projection;
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IQueryable<TOut> Transform(IQueryable<TIn> queryable)
        {
            return queryable.Select(_projection);
        }
    }
}