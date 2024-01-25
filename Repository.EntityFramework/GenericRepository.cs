using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;
using Shared.Query;

namespace Repository.EntityFramework
{
    public class GenericRepository<T> : IGenericRepository<T>
        where T : class
    {
        private readonly CMSDevDbContext _context;
        private readonly IMapper _mapper;

        public GenericRepository(CMSDevDbContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<T> CreateAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);

            return entity;
        }

        public void DeleteAsync(T entity) => _context.Set<T>().Remove(entity);

        public void DeleteList(List<T> entity) => _context.Set<T>().RemoveRange(entity);

        public async Task<bool> Exists(string id)
        {
            var entity = await GetAsync(id);

            return entity != null;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetAsync(string id)
        {
            if (id is null)
            {
                return null;
            }

            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<PagedResult<TResult>> GetPagedResultAsync<TResult>(
            QueryParameters queryParameters
        )
        {
            var totalSize = await _context.Set<T>().CountAsync();

            var item = await _context
                .Set<T>()
                .Skip(queryParameters.StartIndex)
                .Take(queryParameters.PageSize)
                .ProjectTo<TResult>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new PagedResult<TResult>
            {
                Item = item,
                PageNumber = queryParameters.PageNumber,
                RecordNumber = queryParameters.PageSize,
                TotalCount = totalSize
            };
        }

        public IQueryable<T> FindByCondition(
            Expression<Func<T, bool>> expression,
            bool trackChanges
        ) =>
            !trackChanges
                ? _context.Set<T>().Where(expression).AsNoTracking()
                : _context.Set<T>().Where(expression);

        public async Task UpdateAsync(T entity) => _context.Set<T>().Update(entity);
    }
}
