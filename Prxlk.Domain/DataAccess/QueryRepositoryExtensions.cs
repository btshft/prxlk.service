using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Prxlk.Domain.Models;
using Prxlk.Domain.Specifications.Shared;

namespace Prxlk.Domain.DataAccess
{
    public static class QueryRepositoryExtensions
    {
        public static IAsyncEnumerable<TEntity> WhereAsync<TEntity>(
            this IQueryRepository<TEntity> repository, ISpecification<TEntity> specification) 
            where TEntity : GuidEntity
        {
            return repository.WhereAsync(specification, limit: null, offset: null);
        }

        public static Task<TEntity> GetAsync<TEntity>(this IQueryRepository<TEntity> repository, Guid id) 
            where TEntity : GuidEntity
        {
            return repository.GetAsync(id, CancellationToken.None);
        }

        public static Task<int> CountAsync<TEntity>(this IQueryRepository<TEntity> repository)
            where TEntity : GuidEntity
        {
            return repository.CountAsync(Specification<TEntity>.True, CancellationToken.None);
        }
    }
}