using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Prxlk.Domain.DataAccess.QueryTransform
{
    internal class QueryTransformWhere<T> : QueryTransform<T, T> 
        where T : class
    {
        private readonly Expression<Func<T, bool>> _filter;

        public QueryTransformWhere(Expression<Func<T, bool>> filter)
        {
            _filter = filter;
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal override IQueryable<T> TransformInternal(IQueryable<T> queryable)
        {
            // TODO: Expand filter
            return queryable.Where(_filter);
        }
    }
}