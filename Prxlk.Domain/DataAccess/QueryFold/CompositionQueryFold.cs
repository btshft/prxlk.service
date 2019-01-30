using System.Linq;

namespace Prxlk.Domain.DataAccess.QueryFold
{
    internal class CompositionQueryFold<TIn, TOut, TReduce> : IQueryFold<TIn, TReduce> 
        where TOut : class 
        where TIn : class
    {
        private readonly IQueryFold<TIn, IQueryable<TOut>> _inner;
        private readonly IQueryFold<TOut, TReduce> _outer;

        public CompositionQueryFold(IQueryFold<TIn, IQueryable<TOut>> inner, IQueryFold<TOut, TReduce> outer)
        {
            _inner = inner;
            _outer = outer;
        }

        /// <inheritdoc />
        public TReduce FoldQuery(IQueryable<TIn> query)
        {
            return _outer.FoldQuery(_inner.FoldQuery(query));
        }
    }
}