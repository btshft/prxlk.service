using System.Linq;
using System.Runtime.CompilerServices;

namespace Prxlk.Domain.DataAccess.QueryTransform
{   
    internal class QueryTransformTrim<T> : IQueryTransform<T, T> 
        where T : class
    {
        private readonly int? _take;
        private readonly int? _skip;

        public QueryTransformTrim(int? take, int? skip)
        {
            _take = take;
            _skip = skip;
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IQueryable<T> Transform(IQueryable<T> queryable)
        {
            if (_skip.HasValue && _skip > 0)
                queryable = queryable.Skip(_skip.Value);

            if (_take.HasValue && _take > 0)
                queryable = queryable.Take(_take.Value);
            
            return queryable;
        }
    }
}