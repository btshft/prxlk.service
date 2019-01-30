using System.Linq;
using System.Runtime.CompilerServices;

namespace Prxlk.Domain.DataAccess.QueryTransform
{
    internal class QueryTransformIdentity<T> : IQueryTransform<T, T>
        where T : class
    {
        public static QueryTransformIdentity<T> Instance { get; } = new QueryTransformIdentity<T>();

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IQueryable<T> Transform(IQueryable<T> queryable)
        {
            return queryable;
        }
    }
}