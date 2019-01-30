using System;
using System.Threading;
using System.Threading.Tasks;
using Prxlk.Domain.Models;

namespace Prxlk.Domain.DataAccess
{
    public interface IRepository<TEntity> : IQueryRepository<TEntity> 
        where TEntity : Entity
    {
        Guid Add(TEntity entity);
        Task AddAsync(TEntity entity, CancellationToken cancellation);
        void Update(TEntity entity);
        Task UpdateAsync(TEntity entity, CancellationToken cancellation);
        void Remove(Guid id);
        Task RemoveAsync(Guid id, CancellationToken cancellation);
    }
}