using System.Collections.Generic;
using System.Threading.Tasks;
using Prxlk.Domain.DataAccess.QueryTransform;
using Prxlk.Domain.Models;

namespace Prxlk.Domain.DataAccess
{
    public interface IQueryRepository<TEntity> where TEntity : Entity
    {
        IReadOnlyCollection<TOut> QueryTransform<TOut>(QueryTransform<TEntity, TOut> transform) 
            where TOut : class;

        Task<IReadOnlyCollection<TOut>> QueryTransformAsync<TOut>(QueryTransform<TEntity, TOut> transform) 
            where TOut : class;
    }
}