using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Prxlk.Domain.DataAccess.QueryTransform
{
    internal class QueryTransformWhere<T> : IQueryTransform<T, T> 
        where T : class
    {
        private readonly Expression<Func<T, bool>> _filter;

        public QueryTransformWhere(Expression<Func<T, bool>> filter)
        {
            _filter = filter;
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IQueryable<T> Transform(IQueryable<T> queryable)
        {
            return queryable.Where(_filter);
        }
    }
}