using System;
using System.Linq;
using System.Linq.Expressions;

namespace Prxlk.Domain.DataAccess.QueryTransform
{
    public static class QueryTransformExtensions
    {
        public static QueryTransform<T, T> Filter<T>(this QueryTransform<T, T> source, Expression<Func<T, bool>> filter) 
            where T : class
        {
            return new QueryTransformComposition<T, T, T>(
                source, new QueryTransformWhere<T>(filter));
        }

        public static QueryTransform<T, T> Trim<T>(this QueryTransform<T, T> source, int take, int skip) 
            where T : class
        {
            return new QueryTransformComposition<T, T, T>(source, new QueryTransformTrim<T>(take, skip));
        }
        
        public static QueryTransform<T, T> Take<T>(this QueryTransform<T, T> source, int take) 
            where T : class
        {
            return new QueryTransformComposition<T, T, T>(source, new QueryTransformTrim<T>(take, skip: null));
        }

        public static QueryTransform<T, T> Skip<T>(this QueryTransform<T, T> source, int skip) 
            where T : class
        {
            return new QueryTransformComposition<T, T, T>(source, new QueryTransformTrim<T>(take: null, skip: skip));
        }
        
        public static QueryTransform<T1, T2> Project<T1, T2>(this QueryTransform<T1, T1> source,
            Expression<Func<T1, T2>> projector) 
            where T2 : class 
            where T1 : class
        {
            return new QueryTransformComposition<T1, T1, T2>(source, new QueryTransformProject<T1, T2>(projector));
        }
    }
}