using System.Linq;
using System.Runtime.CompilerServices;

namespace Prxlk.Domain.DataAccess.QueryTransform
{
    internal class QueryTransformIdentity<T> : QueryTransform<T, T>
        where T : class
    {
        public static QueryTransformIdentity<T> Instance { get; } = new QueryTransformIdentity<T>();

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal override IQueryable<T> TransformInternal(IQueryable<T> queryable)
        {
            return queryable;
        }
    }
}