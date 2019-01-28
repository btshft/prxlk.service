using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Prxlk.Domain.DataAccess;
using Prxlk.Domain.Models;
using Prxlk.Domain.Specifications.Shared;

namespace Prxlk.Data.EntityFramework
{
    public class EntityFrameworkRepository<TEntity> : IQueryRepository<TEntity>, IRepository<TEntity> 
        where TEntity : GuidEntity
    {
        private readonly ProxyDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public EntityFrameworkRepository(ProxyDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        /// <inheritdoc />
        public IAsyncEnumerable<TEntity> WhereAsync(ISpecification<TEntity> specification, int? limit, int? offset)
        {
            var query = _dbSet.Where(specification.AsExpression());
            if (offset.HasValue)
                query = query.Skip(offset.Value);

            if (limit.HasValue)
                query = query.Take(limit.Value);
            
            return query.AsNoTracking().ToAsyncEnumerable();
        }

        /// <inheritdoc />
        public IEnumerable<TEntity> Where(ISpecification<TEntity> specification, int? limit, int? offset)
        {
            var query = _dbSet.Where(specification.AsExpression());
            if (offset.HasValue)
                query = query.Skip(offset.Value);

            if (limit.HasValue)
                query = query.Take(limit.Value);
            
            return query.AsNoTracking().AsEnumerable();
        }

        /// <inheritdoc />
        public TEntity Get(Guid id)
        {
            return _dbSet.Find(id);
        }

        /// <inheritdoc />
        public Task<TEntity> GetAsync(Guid id, CancellationToken cancellation)
        {
            return _dbSet.FindAsync(id, cancellation);
        }

        /// <inheritdoc />
        public Task<int> CountAsync(ISpecification<TEntity> specification, CancellationToken cancellation)
        {
            return _dbSet.CountAsync(specification.AsExpression(), cancellationToken: cancellation);
        }

        /// <inheritdoc />
        public Guid Add(TEntity entity)
        {
            return _dbSet.Add(entity).Entity.Id;
        }

        /// <inheritdoc />
        public async Task<Guid> AddAsync(TEntity entity, CancellationToken cancellation)
        {
            var entry = await _dbSet.AddAsync(entity, cancellation);
            return entry.Entity.Id;
        }

        /// <inheritdoc />
        public void Update(TEntity entity)
        {
            _dbContext.Update(entity);
        }

        public Task UpdateAsync(TEntity entity, CancellationToken cancellation)
        {
            Update(entity);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public void Remove(Guid id)
        {
            _dbContext.Remove(_dbSet.Find(id));
        }

        /// <inheritdoc />
        public async Task RemoveAsync(Guid id, CancellationToken cancellation)
        {
            var entity = await GetAsync(id, cancellation);
            if (entity != null)
                _dbSet.Remove(entity);
        }
    }
}