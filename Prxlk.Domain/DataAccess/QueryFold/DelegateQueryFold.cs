using System;
using System.Linq;

namespace Prxlk.Domain.DataAccess.QueryFold
{
    internal class DelegateQueryFold<TIn, TOut> : IQueryFold<TIn, TOut>
        where TIn : class
    {
        private readonly Func<IQueryable<TIn>, TOut> _fold;

        public DelegateQueryFold(Func<IQueryable<TIn>, TOut> fold)
        {
            _fold = fold;
        }

        /// <inheritdoc />
        public TOut FoldQuery(IQueryable<TIn> query)
        {
            return _fold(query);
        }
    }
}