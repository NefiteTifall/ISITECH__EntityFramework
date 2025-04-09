using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ISITECH__EventsArea.Domain.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
	// Read
	Task<TEntity> GetByIdAsync(int id);
	Task<IEnumerable<TEntity>> GetAllAsync();
	Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
	Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        
	// Create
	Task AddAsync(TEntity entity);
	Task AddRangeAsync(IEnumerable<TEntity> entities);
        
	// Update
	void Update(TEntity entity);
	void UpdateRange(IEnumerable<TEntity> entities);
        
	// Delete
	void Remove(TEntity entity);
	void RemoveRange(IEnumerable<TEntity> entities);
        
	// Count
	Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null);
        
	// Exists
	Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
        
	// Get paginated results
	Task<(IEnumerable<TEntity> Items, int TotalCount)> GetPagedAsync(
		int pageIndex, 
		int pageSize, 
		Expression<Func<TEntity, bool>> filter = null,
		Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
}