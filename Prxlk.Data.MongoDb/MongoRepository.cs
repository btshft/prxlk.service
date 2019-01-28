using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using Prxlk.Domain.DataAccess;
using Prxlk.Domain.Models;
using Prxlk.Domain.Specifications.Shared;

namespace Prxlk.Data.MongoDb
{
    
    
    public class MongoRepository<TEntity> : IQueryRepository<TEntity>, IRepository<TEntity> 
        where TEntity : GuidEntity
    {
        private readonly IMongoCollection<TEntity> _collection;

        public MongoRepository(IMongoDatabaseProvider provider)
        {
            _collection = provider.GetDatabase().GetCollection<TEntity>(typeof(TEntity).Name);
        }

        /// <inheritdoc />
        public IAsyncEnumerable<TEntity> WhereAsync(ISpecification<TEntity> specification, int? limit, int? offset)
        {
            var query = _collection.AsQueryable().Where(specification.AsExpression());

            if (offset.HasValue)
                query = query.Skip(offset.Value);

            if (limit.HasValue)
                query = query.Take(limit.Value);

            return query.ToAsyncEnumerable();
        }

        /// <inheritdoc />
        public IEnumerable<TEntity> Where(ISpecification<TEntity> specification, int? limit, int? offset)
        {
            var query = _collection.AsQueryable().Where(specification.AsExpression());

            if (offset.HasValue)
                query = query.Skip(offset.Value);

            if (limit.HasValue)
                query = query.Take(limit.Value);
            
            return query.AsEnumerable();
        }

        /// <inheritdoc />
        public TEntity Get(Guid id)
        {
            return _collection.Find(e => e.Id == id).FirstOrDefault();
        }

        /// <inheritdoc />
        public Task<TEntity> GetAsync(Guid id, CancellationToken cancellation)
        {
            return _collection.Find(e => e.Id == id).FirstOrDefaultAsync(cancellation);
        }

        /// <inheritdoc />
        public async Task<int> CountAsync(ISpecification<TEntity> specification, CancellationToken cancellation)
        {
            var longCount = await _collection.CountDocumentsAsync(specification.AsExpression(), cancellationToken: cancellation);
            return (int) longCount;
        }

        /// <inheritdoc />
        public Guid Add(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
             
            _collection.InsertOne(entity);
            return entity.Id;
        }

        /// <inheritdoc />
        public async Task<Guid> AddAsync(TEntity entity, CancellationToken cancellation)
        {
            await _collection.InsertOneAsync(entity, cancellationToken: cancellation);
            return entity.Id;
        }

        /// <inheritdoc />
        public void Update(TEntity entity)
        {
            _collection.ReplaceOne(a => a.Id == entity.Id, entity);
        }

        /// <inheritdoc />
        public Task UpdateAsync(TEntity entity, CancellationToken cancellation)
        {
            return _collection.ReplaceOneAsync(a => a.Id == entity.Id, entity, cancellationToken: cancellation);
        }

        /// <inheritdoc />
        public void Remove(Guid id)
        {
            _collection.DeleteOne(f => f.Id == id);
        }

        /// <inheritdoc />
        public Task RemoveAsync(Guid id, CancellationToken cancellation)
        {
            return _collection.DeleteOneAsync(f => f.Id == id, cancellationToken: cancellation);
        }
    }
}