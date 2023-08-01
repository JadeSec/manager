using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace App.Infra.Integration.Database.Interfaces
{
    public interface IRepository<TEntity>
    {
        public void Create(TEntity obj);

        public void Delete(TEntity obj);

        public void Update(TEntity obj);

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);

        public Task<int> SaveAsync();
    }
}