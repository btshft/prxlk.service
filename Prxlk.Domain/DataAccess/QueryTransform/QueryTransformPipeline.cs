namespace Prxlk.Domain.DataAccess.QueryTransform
{
    public static class QueryTransformPipeline<T> where T :  class
    {
        public static QueryTransform<T, T> Create()
            => QueryTransformIdentity<T>.Instance;
    }
}