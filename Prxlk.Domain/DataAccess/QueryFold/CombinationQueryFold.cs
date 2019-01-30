using System;
using System.Linq;

namespace Prxlk.Domain.DataAccess.QueryFold
{
    internal class CombinationQueryFold<T1, T2, T3> : IQueryFold<T1, T3> 
        where T1 : class
    {
        private readonly IQueryFold<T1, T2> _inner;
        private readonly Func<T2, IQueryable<T1>, T3> _aggregate;

        public CombinationQueryFold(IQueryFold<T1, T2> inner, Func<T2, IQueryable<T1>, T3> aggregate)
        {
            _inner = inner;
            _aggregate = aggregate;
        }

        /// <inheritdoc />
        public T3 FoldQuery(IQueryable<T1> query)
        {
            return _aggregate(_inner.FoldQuery(query), query);
        }
    }
}