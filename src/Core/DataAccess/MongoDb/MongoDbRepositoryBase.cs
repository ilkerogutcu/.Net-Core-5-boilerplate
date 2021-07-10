using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.CrossCuttingConcerns.Logging.Serilog.ConfigurationModels;
using Core.DataAccess.MongoDb.Configuration;
using Core.Entities.MongoDb;
using Core.Utilities.IoC;
using Core.Utilities.Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Core.DataAccess.MongoDb
{
    public abstract class MongoDbRepositoryBase<T> : IDocumentDbRepository<T>
        where T : DocumentEntity
    {
        private readonly IMongoCollection<T> _collection;
        private string CollectionName { get; }

        protected MongoDbRepositoryBase(string collectionName)
        {
            var configuration = ServiceTool.ServiceProvider.GetService<IConfiguration>();
            var config = configuration.GetSection("MongoDatabaseConfig").Get<MongoDatabaseConfig>();
            CollectionName = collectionName;
            ConnectionSettingControl(config);
            var client = new MongoClient(config.ConnectionString);
            var database = client.GetDatabase(config.DatabaseName);
            _collection = database.GetCollection<T>(collectionName);
        }

        private void ConnectionSettingControl(MongoDatabaseConfig config)
        {
            if (config != null &&
                (string.IsNullOrEmpty(CollectionName) || string.IsNullOrEmpty(config.DatabaseName)))
                throw new Exception(MongoDbMessages.NullOrEmptyMessage);
        }

        public void Add(T entity)
        {
            var options = new InsertOneOptions {BypassDocumentValidation = false};
            _collection.InsertOne(entity, options);
        }

        public void AddMany(IEnumerable<T> entities)
        {
            var options = new BulkWriteOptions {IsOrdered = false, BypassDocumentValidation = false};
            _collection.BulkWriteAsync((IEnumerable<WriteModel<T>>) entities, options);
        }

        public IQueryable<T> GetList(Expression<Func<T, bool>> predicate = null)
        {
            return predicate == null
                ? _collection.AsQueryable()
                : _collection.AsQueryable().Where(predicate);
        }

        public T GetById(ObjectId id)
        {
            return _collection.Find(x => x.Id == id).FirstOrDefault();
        }

        public virtual void Update(ObjectId id, T record)
        {
            _collection.FindOneAndReplace(x => x.Id == id, record);
        }

        public virtual void Update(T record, Expression<Func<T, bool>> predicate)
        {
            _collection.FindOneAndReplace(predicate, record);
        }

        public void DeleteById(ObjectId id)
        {
            _collection.FindOneAndDelete(x => x.Id == id);
        }

        public void Delete(T record)
        {
            _collection.FindOneAndDelete(x => x.Id == record.Id);
        }

        public bool Any(Expression<Func<T, bool>> predicate = null)
        {
            var data = predicate == null
                ? _collection.AsQueryable()
                : _collection.AsQueryable().Where(predicate);
            return data.FirstOrDefault() != null;
        }

        public async Task<IQueryable<T>> GetListAsync(Expression<Func<T, bool>> predicate = null)
        {
            return await Task.Run(() => predicate == null
                ? _collection.AsQueryable()
                : _collection.AsQueryable().Where(predicate));
        }

        public async Task<T> GetByIdAsync(ObjectId id)
        {
            return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddAsync(T entity)
        {
            var options = new InsertOneOptions {BypassDocumentValidation = false};
            await _collection.InsertOneAsync(entity, options);
        }

        public async Task AddManyAsync(IEnumerable<T> entities)
        {
            var options = new BulkWriteOptions {IsOrdered = false, BypassDocumentValidation = false};
            await _collection.BulkWriteAsync((IEnumerable<WriteModel<T>>) entities, options);
        }

        public async Task UpdateAsync(ObjectId id, T record)
        {
            await _collection.FindOneAndReplaceAsync(x => x.Id == id, record);
        }

        public async Task UpdateAsync(T record, Expression<Func<T, bool>> predicate)
        {
            await _collection.FindOneAndReplaceAsync(predicate, record);
        }

        public async Task DeleteByIdAsync(ObjectId id)
        {
            await _collection.FindOneAndDeleteAsync(x => x.Id == id);
        }

        public async Task DeleteAsync(T record)
        {
            await _collection.FindOneAndDeleteAsync(x => x.Id == record.Id);
        }
    }
}