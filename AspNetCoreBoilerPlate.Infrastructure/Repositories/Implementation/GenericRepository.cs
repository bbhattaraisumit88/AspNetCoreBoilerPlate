using AspNetCoreBoilerPlate.EFCore;
using AspNetCoreBoilerPlate.Infrastructure.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AspNetCoreBoilerPlate.Infrastructure.Repositories.Implementation
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private DbSet<T> _dbSet;
        private readonly ApplicationDbContext _dbContext;
        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException("Context was not supplied");
            _dbSet = _dbContext.Set<T>();
        }

        public IQueryable<T> GetAll(
             Expression<Func<T, bool>> filter = null,
             Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
             List<Expression<Func<T, object>>> includes = null,
             int? page = null,
             int? pageSize = null)
        {
            IQueryable<T> query = _dbSet;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (page != null && pageSize != null)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }
            return query;
        }

        public async Task<IEnumerable<T>> GetAllAsync(
           Expression<Func<T, bool>> filter = null,
           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
           List<Expression<Func<T, object>>> includes = null,
           int? page = null,
           int? pageSize = null)
        {
            return await GetAll(filter, orderBy, includes, page, pageSize).ToListAsync();
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> predicate) => _dbSet.Where(predicate);

        public void Insert(T entity)
        {
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Added;
        }

        public void InsertRange(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);
        }

        public void Update(T entity)
        {
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }
        public void Delete(object id)
        {
            T entityToDelete = _dbSet.Find(id);
            Delete(entityToDelete);
        }

        public void Delete(Guid userId, List<Guid> roleId)
        {
            foreach (var item in roleId)
            {
                T entityToDelete = _dbSet.Find(userId, item);
                Delete(entityToDelete);
            }
        }

        public void DeleteAsync(object id)
        {
            Task<T> entityToDelete = _dbSet.FindAsync(id);
            Delete(entityToDelete);
        }

        public void Delete(T entity)
        {
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Deleted;
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public IQueryable<T> RawSql(string query, params object[] parameters)
        {
            return _dbSet.FromSql(query, parameters).AsQueryable();
        }

    }

}
