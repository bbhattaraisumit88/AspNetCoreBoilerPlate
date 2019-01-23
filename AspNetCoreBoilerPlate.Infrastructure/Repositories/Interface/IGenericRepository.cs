using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AspNetCoreBoilerPlate.Infrastructure.Repositories.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            List<Expression<Func<T, object>>> includes = null,
            int? page = null,
            int? pageSize = null);
        Task<IEnumerable<T>> GetAllAsync(
          Expression<Func<T, bool>> filter = null,
          Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
          List<Expression<Func<T, object>>> includes = null,
          int? page = null,
          int? pageSize = null);
        IQueryable<T> Find(Expression<Func<T, bool>> predicate);
        void Insert(T entity);
        void InsertRange(IEnumerable<T> entities);
        void Update(T entity);
        void Delete(Guid userId, List<Guid> roles);
        void Delete(object id);
        void Delete(T entity);
        void DeleteAsync(object id);
        void DeleteRange(IEnumerable<T> entities);
        IQueryable<T> RawSql(string query, params object[] parameters);
    }
}
