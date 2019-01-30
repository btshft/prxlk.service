namespace Prxlk.Domain.DataAccess.QueryTransform
{
    public interface IQueryTransformBuilder<TIn, TOut> 
        where TIn : class
        where TOut : class
    {
        QueryTransform<TIn, TOut> CreateQueryTransform();
    }
    
    public interface IQueryTransformBuilder<TIn, TOut, in TArg> 
        where TIn : class
        where TOut : class
    {
        QueryTransform<TIn, TOut> CreateQueryTransform(TArg arg);
    }
}