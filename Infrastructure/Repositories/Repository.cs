using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ISITECH__EventsArea.Domain.Interfaces;
using ISITECH__EventsArea.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ISITECH__EventsArea.Infrastructure.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly EventsAreasDbContext _context;
        protected readonly DbSet<TEntity> _entities;

        public Repository(EventsAreasDbContext context)
        {
            _context = context;
            _entities = context.Set<TEntity>();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _entities.FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _entities.Where(predicate).ToListAsync();
        }

        public async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _entities.SingleOrDefaultAsync(predicate);
        }

        public async Task AddAsync(TEntity entity)
        {
            await _entities.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _entities.AddRangeAsync(entities);
        }

        public void Update(TEntity entity)
        {
            _entities.Update(entity);
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            _entities.UpdateRange(entities);
        }

        public void Remove(TEntity entity)
        {
            _entities.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _entities.RemoveRange(entities);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            if (predicate == null)
                return await _entities.CountAsync();
            
            return await _entities.CountAsync(predicate);
        }

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _entities.AnyAsync(predicate);
        }

        public async Task<(IEnumerable<TEntity> Items, int TotalCount)> GetPagedAsync(
            int pageIndex, 
            int pageSize, 
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = _entities;

            if (filter != null)
                query = query.Where(filter);

            int totalCount = await query.CountAsync();

            if (orderBy != null)
                query = orderBy(query);

            var items = await query.Skip((pageIndex - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToListAsync();

            return (items, totalCount);
        }
    }
}