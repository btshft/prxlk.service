using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Prxlk.Domain.Models;
using Prxlk.Domain.Specifications.Shared;

namespace Prxlk.Domain.DataAccess
{
    public interface IQueryRepository<TEntity> where TEntity : GuidEntity
    {
        IAsyncEnumerable<TEntity> WhereAsync(ISpecification<TEntity> specification, int? limit, int? offset);
        IEnumerable<TEntity> Where(ISpecification<TEntity> specification, int? limit, int? offset);
        TEntity Get(Guid id);
        Task<TEntity> GetAsync(Guid id, CancellationToken cancellation);
        Task<int> CountAsync(ISpecification<TEntity> specification, CancellationToken cancellation);
    }
}