using System;
using System.Linq;

namespace Prxlk.Domain.DataAccess.QueryFold
{
    public static class QueryFoldExtensions
    {
        public static IQueryFold<T1, T3> Compose<T1, T2, T3>(
            this IQueryFold<T1, IQueryable<T2>> source, Func<IQueryable<T2>, T3> reduce) 
            where T1 : class 
            where T2 : class
        {
            return new CompositionQueryFold<T1, T2, T3>(source, 
                new DelegateQueryFold<T2, T3>(reduce));
        }

        public static IQueryFold<T1, T3> Combine<T1, T2, T3>(
            this IQueryFold<T1, T2> source, Func<T2, IQueryable<T1>, T3> aggregate) 
            where T1 : class
        {
            return new CombinationQueryFold<T1, T2, T3>(source, aggregate);
        }
    }
}