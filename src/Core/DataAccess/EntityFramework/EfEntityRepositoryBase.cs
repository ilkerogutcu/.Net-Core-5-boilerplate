﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.DataAccess.EntityFramework
{
    public class EfEntityRepositoryBase<TEntity, TContext>
        : IEntityRepository<TEntity>
        where TEntity : class, IEntity
        where TContext : DbContext
    {
        public EfEntityRepositoryBase(TContext context)
        {
            Context = context;
        }

        private TContext Context { get; }

        public TEntity Add(TEntity entity)
        {
            return Context.Add(entity).Entity;
        }

        public TEntity Update(TEntity entity)
        {
            Context.Update(entity);
            return entity;
        }

        public void Delete(TEntity entity)
        {
            Context.Remove(entity);
        }

        public TEntity Get(Expression<Func<TEntity, bool>> expression)
        {
            return Context.Set<TEntity>().FirstOrDefault(expression);
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await Context.Set<TEntity>().AsQueryable().FirstOrDefaultAsync(expression);
        }

        public List<TEntity> GetList(Expression<Func<TEntity, bool>> expression = null)
        {
            return expression == null
                ? Context.Set<TEntity>().AsNoTracking().ToList()
                : Context.Set<TEntity>().Where(expression).AsNoTracking().ToList();
        }

        public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> expression = null)
        {
            return expression == null
                ? await Context.Set<TEntity>().ToListAsync()
                : await Context.Set<TEntity>().Where(expression).ToListAsync();
        }

        public int SaveChanges()
        {
            return Context.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return Context.SaveChangesAsync();
        }

        public IQueryable<TEntity> Query()
        {
            return Context.Set<TEntity>();
        }

        public Task<int> Execute(FormattableString interpolatedQueryString)
        {
            return Context.Database.ExecuteSqlInterpolatedAsync(interpolatedQueryString);
        }

        /// <summary>
        ///     Transactional operations is prohibited when working with InMemoryDb!
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="action"></param>
        /// <param name="successAction"></param>
        /// <param name="exceptionAction"></param>
        /// <returns></returns>
        public TResult InTransaction<TResult>(Func<TResult> action, Action successAction = null,
            Action<Exception> exceptionAction = null)
        {
            var result = default(TResult);
            try
            {
                if (Context.Database.ProviderName.EndsWith("InMemory"))
                {
                    result = action();
                    SaveChanges();
                }
                else
                {
                    using var tx = Context.Database.BeginTransaction();
                    try
                    {
                        result = action();
                        SaveChanges();
                        tx.Commit();
                    }
                    catch (Exception)
                    {
                        tx.Rollback();
                        throw;
                    }
                }

                successAction?.Invoke();
            }
            catch (Exception ex)
            {
                if (exceptionAction is null) throw;

                exceptionAction(ex);
            }

            return result;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            var result = await Context.AddAsync(entity);
            return result.Entity;
        }

        public async Task<int> GetCountAsync(Expression<Func<TEntity, bool>> expression = null)
        {
            if (expression is null)
                return await Context.Set<TEntity>().CountAsync();
            return await Context.Set<TEntity>().CountAsync(expression);
        }

        public int GetCount(Expression<Func<TEntity, bool>> expression = null)
        {
            return expression == null ? Context.Set<TEntity>().Count() : Context.Set<TEntity>().Count(expression);
        }
    }
}