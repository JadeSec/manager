using SqlKata.Execution;
using App.Infra.Bootstrap;
using Microsoft.EntityFrameworkCore;
using App.Infra.Integration.Database.Interfaces;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using System;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Common;
using System.Collections.Generic;

namespace App.Infra.Integration.Database
{
    public abstract class RepositoryService<TProvider, TEntity> : IRepository<TEntity>
       where TProvider : IProvider
       where TEntity : class
    {
        protected readonly IProvider Provider;
        protected readonly DbContext Context;

        protected DbConnection Connection
            => Provider.Connection;

        /// <summary>
        /// https://dapper-tutorial.net/
        /// </summary>
        protected DbConnection Dapper
            => Connection;

        /// <summary>
        /// https://github.com/sqlkata/querybuilder
        /// </summary>
        protected QueryFactory SqlKata
            => new(Connection, Provider.Compiler);

        protected string ConnectionString
            => Provider.ConnectionString;

        public RepositoryService()
        {
            var provider = Ioc.Get<IScoped<TProvider>>();

            Provider = (IProvider)provider;
            Context = (DbContext)provider;
        }

        public void Create(TEntity obj)
           => Context.Set<TEntity>()
                     .Add(obj);

        public async Task<bool> CreateAndSaveAsync(TEntity obj)
        {
            Create(obj);
            return await SaveAsync() > 0;           
        }

        public async Task<bool> UpdateAndSaveAsync(TEntity obj)
        {
            Update(obj);
            return await SaveAsync() > 0;
        }

        public void Delete(TEntity obj)
           => Context.Set<TEntity>()
                     .Remove(obj);

        public void Update(TEntity obj)
           => Context.Update(obj);

        public async Task<int> SaveAsync()
           => await Context.SaveChangesAsync();

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
           =>  Context.Set<TEntity>()
                      .AsNoTracking()
                      .Where(predicate);

        public async Task<List<TEntity>> ToListAsync()
           => await Context.Set<TEntity>()
                           .AsNoTracking()
                           .ToListAsync();

        public bool Any(Expression<Func<TEntity, bool>> predicate)
           => Context.Set<TEntity>()
                     .Any(predicate);

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
           => await Context.Set<TEntity>()
                     .AnyAsync(predicate);

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            var context = Context.Set<TEntity>();

            foreach (var include in includes)
                context.Include(include);

            return context.Where(predicate);
        }

        public async Task<IDbContextTransaction> TransactionAsync()
        {
            var context = (DbContext)Ioc.Get<ISingleton<TProvider>>();

            return await context.Database.BeginTransactionAsync();
        }
    }
}