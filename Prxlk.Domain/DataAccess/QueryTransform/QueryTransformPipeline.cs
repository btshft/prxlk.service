namespace Prxlk.Domain.DataAccess.QueryTransform
{
    public static class QueryTransformPipeline<T> where T :  class
    {
        public static IQueryTransform<T, T> Create()
            => QueryTransformIdentity<T>.Instance;
    }
}