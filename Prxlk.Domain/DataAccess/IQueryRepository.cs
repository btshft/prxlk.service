using System.Collections.Generic;
using System.Threading.Tasks;
using Prxlk.Domain.DataAccess.QueryFold;
using Prxlk.Domain.DataAccess.QueryTransform;
using Prxlk.Domain.Models;

namespace Prxlk.Domain.DataAccess
{   
    public interface IQueryRepository<out TEntity> where TEntity : Entity
    {
        IReadOnlyCollection<TOut> Query<TOut>(IQueryTransform<TEntity, TOut> transform) 
            where TOut : class;

        Task<IReadOnlyCollection<TOut>> QueryAsync<TOut>(IQueryTransform<TEntity, TOut> transform) 
            where TOut : class;

        TOut QueryFold<TOut>(IQueryFold<TEntity, TOut> fold);
    }
}