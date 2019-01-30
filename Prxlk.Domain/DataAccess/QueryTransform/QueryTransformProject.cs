using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Prxlk.Domain.DataAccess.QueryTransform
{
    internal class QueryTransformProject<TIn, TOut> : QueryTransform<TIn, TOut> 
        where TIn : class where TOut : class
    {
        private readonly Expression<Func<TIn, TOut>> _projection;

        public QueryTransformProject(Expression<Func<TIn, TOut>> projection)
        {
            _projection = projection;
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal override IQueryable<TOut> TransformInternal(IQueryable<TIn> queryable)
        {
            // TODO: Expand projection
            // https://benjii.me/2018/01/expression-projection-magic-entity-framework-core/
            return queryable.Select(_projection);
        }
    }
}