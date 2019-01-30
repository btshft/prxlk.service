using System.Linq;

namespace Prxlk.Domain.DataAccess.QueryFold
{
    public interface IQueryFold<in TIn, out TOut> 
        where TIn : class
    {
        TOut FoldQuery(IQueryable<TIn> query);
    }
}