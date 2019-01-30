using System.Linq;

namespace Prxlk.Domain.DataAccess.QueryTransform
{
    public interface IQueryTransform<in TIn, out TOut> where TOut : class
    {
        IQueryable<TOut> Transform(IQueryable<TIn> queryable);
    }
}