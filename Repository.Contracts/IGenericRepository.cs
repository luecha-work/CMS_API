using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Shared.Query;

namespace Repository.Contracts
{
    public interface IGenericRepository<T>
        where T : class
    {
        Task<T> GetAsync(string id);
        Task<List<T>> GetAllAsync();
        Task<PagedResult<TResult>> GetPagedResultAsync<TResult>(QueryParameters queryParameters);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges);
        Task<T> CreateAsync(T entity);
        void DeleteAsync(T entity);
        void DeleteList(List<T> entity);
        Task UpdateAsync(T entity);
        Task<bool> Exists(string id);
    }
}
