using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using Prxlk.Domain.DataAccess;
using Prxlk.Domain.DataAccess.QueryFold;
using Prxlk.Domain.DataAccess.QueryTransform;
using Prxlk.Domain.Models;

namespace Prxlk.Data.MongoDb
{
    public class MongoRepository<TEntity> : IQueryRepository<TEntity>, IRepository<TEntity> 
        where TEntity : Entity<Guid>
    {
        private readonly IMongoCollection<TEntity> _collection;

        public MongoRepository(IMongoDatabaseProvider provider)
        {
            _collection = provider.GetDatabase().GetCollection<TEntity>(typeof(TEntity).Name);
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
        public async Task AddAsync(TEntity entity, CancellationToken cancellation)
        {
            await _collection.InsertOneAsync(entity, cancellationToken: cancellation);
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

        /// <inheritdoc />
        public IReadOnlyCollection<TOut> Query<TOut>(IQueryTransform<TEntity, TOut> transform) 
            where TOut : class
        {
            return transform.Transform(_collection.AsQueryable()).ToArray();
        }

        /// <inheritdoc />
        public async Task<IReadOnlyCollection<TOut>> QueryAsync<TOut>(IQueryTransform<TEntity, TOut> transform) 
            where TOut : class
        {
            return await transform.Transform(_collection.AsQueryable())
                .ToAsyncEnumerable()
                .ToArray();
        }

        /// <inheritdoc />
        public TOut QueryFold<TOut>(IQueryFold<TEntity, TOut> fold)
        {
            return fold.FoldQuery(_collection.AsQueryable());
        }
    }
}