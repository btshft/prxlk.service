using System.Linq;

namespace Prxlk.Domain.DataAccess.QueryFold
{
    public static class QueryFoldPipeline
    {
        public static IQueryFold<T, IQueryable<T>> Create<T>() where T : class 
            => IdentityQueryFold<T>.Instance;
    }
}