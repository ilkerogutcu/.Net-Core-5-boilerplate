using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Entities.MongoDb;
using MongoDB.Bson;

namespace Core.DataAccess
{
    public interface IDocumentDbRepository<T> where T : DocumentEntity
    {
        void Add(T entity);
        void AddMany(IEnumerable<T> entities);
        IQueryable<T> GetList(Expression<Func<T, bool>> predicate = null);
        T GetById(ObjectId id);
        void Update(ObjectId id, T record);
        void Update(T record, Expression<Func<T, bool>> predicate);
        void DeleteById(ObjectId id);
        void Delete(T record);
        bool Any(Expression<Func<T, bool>> predicate = null);
        Task<IQueryable<T>> GetListAsync(Expression<Func<T, bool>> predicate = null);
        Task<T> GetByIdAsync(ObjectId id);
        Task AddAsync(T entity);
        Task AddManyAsync(IEnumerable<T> entities);
        Task UpdateAsync(ObjectId id, T record);
        Task UpdateAsync(T record, Expression<Func<T, bool>> predicate);
        Task DeleteByIdAsync(ObjectId id);
        Task DeleteAsync(T record);
    }
}