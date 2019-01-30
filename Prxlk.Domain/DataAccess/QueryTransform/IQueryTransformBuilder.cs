namespace Prxlk.Domain.DataAccess.QueryTransform
{
    public interface IQueryTransformBuilder<in TIn, out TOut> 
        where TIn : class
        where TOut : class
    {
        IQueryTransform<TIn, TOut> CreateQueryTransform();
    }
    
    public interface IQueryTransformBuilder<in TIn, out TOut, in TArg> 
        where TIn : class
        where TOut : class
    {
        IQueryTransform<TIn, TOut> CreateQueryTransform(TArg arg);
    }
}