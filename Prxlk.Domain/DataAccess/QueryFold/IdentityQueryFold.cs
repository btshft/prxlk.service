using System.Linq;

namespace Prxlk.Domain.DataAccess.QueryFold
{
    internal class IdentityQueryFold<T> : IQueryFold<T, IQueryable<T>> 
        where T : class
    {
        public static IdentityQueryFold<T> Instance { get; } = new IdentityQueryFold<T>();
        
        public IQueryable<T> FoldQuery(IQueryable<T> query)
        {
            return query;
        }
    }
}