using System.Linq;
using System.Runtime.CompilerServices;

namespace Prxlk.Domain.DataAccess.QueryTransform
{
    internal class QueryTransformComposition<TIn, TOut, TResult> : IQueryTransform<TIn, TResult> 
        where TIn : class 
        where TResult : class
        where TOut : class
    {
        private readonly IQueryTransform<TIn, TOut> _inner;
        private readonly IQueryTransform<TOut, TResult> _outer;

        internal QueryTransformComposition(IQueryTransform<TIn, TOut> inner, IQueryTransform<TOut, TResult> outer)
        {
            _inner = inner;
            _outer = outer;
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IQueryable<TResult> Transform(IQueryable<TIn> queryable)
        {
            return _outer.Transform(_inner.Transform(queryable));
        }
    }
}