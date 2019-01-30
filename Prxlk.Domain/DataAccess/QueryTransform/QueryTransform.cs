using System.Linq;

namespace Prxlk.Domain.DataAccess.QueryTransform
{
    public abstract class QueryTransform<TIn, TOut> where TOut : class
    {
        public IQueryable<TOut> Transform(IQueryable<TIn> queryable) 
            => TransformInternal(queryable);

        /// <summary>
        /// Restrict creation of custom transform rules outside of domain logic.
        /// </summary>
        internal abstract IQueryable<TOut> TransformInternal(IQueryable<TIn> queryable);
    }
}