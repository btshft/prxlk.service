using System.Linq;
using System.Runtime.CompilerServices;

namespace Prxlk.Domain.DataAccess.QueryTransform
{
    internal class QueryTransformComposition<TIn, TOut, TResult> : QueryTransform<TIn, TResult> 
        where TIn : class 
        where TResult : class
        where TOut : class
    {
        private readonly QueryTransform<TIn, TOut> _inner;
        private readonly QueryTransform<TOut, TResult> _outer;

        internal QueryTransformComposition(QueryTransform<TIn, TOut> inner, QueryTransform<TOut, TResult> outer)
        {
            _inner = inner;
            _outer = outer;
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal override IQueryable<TResult> TransformInternal(IQueryable<TIn> queryable)
        {
            return _outer.Transform(_inner.Transform(queryable));
        }
    }
}