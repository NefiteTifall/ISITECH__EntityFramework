using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISITECH__EventsArea.Domain.Entities;
using ISITECH__EventsArea.Domain.Interfaces;
using ISITECH__EventsArea.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ISITECH__EventsArea.Infrastructure.Repositories
{
    public class EventCategoryRepository : Repository<EventCategory>, IEventCategoryRepository
    {
        public EventCategoryRepository(EventsAreasDbContext context) 
            : base(context)
        {
        }

        public async Task<EventCategory> GetCategoryWithEventsAsync(int id)
        {
            return await _entities
                .Include(c => c.Events)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<EventCategory>> GetAllCategoriesWithEventCountAsync()
        {
            return await _entities
                .Include(c => c.Events)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<(IEnumerable<EventCategory> Categories, int TotalCount)> GetPagedCategoriesWithEventCountAsync(
            string searchTerm,
            int pageIndex,
            int pageSize)
        {
            IQueryable<EventCategory> query = _entities
                .Include(c => c.Events)
                .AsNoTracking();

            // Apply search filter if provided
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(c => c.Name.Contains(searchTerm) || 
                                        c.Description.Contains(searchTerm));
            }

            // Get total count
            int totalCount = await query.CountAsync();

            // Apply pagination
            var categories = await query
                .OrderBy(c => c.Name)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (categories, totalCount);
        }
    }
}