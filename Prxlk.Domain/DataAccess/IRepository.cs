using System;
using System.Threading;
using System.Threading.Tasks;
using Prxlk.Domain.Models;

namespace Prxlk.Domain.DataAccess
{
    public interface IRepository<in TEntity> where TEntity : GuidEntity
    {
        Guid Add(TEntity entity);
        Task<Guid> AddAsync(TEntity entity, CancellationToken cancellation);
        void Update(TEntity entity);
        Task UpdateAsync(TEntity entity, CancellationToken cancellation);
        void Remove(Guid id);
        Task RemoveAsync(Guid id, CancellationToken cancellation);
    }
}