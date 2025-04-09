using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISITECH__EventsArea.Domain.Entities;
using ISITECH__EventsArea.Domain.Interfaces;
using ISITECH__EventsArea.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ISITECH__EventsArea.Infrastructure.Repositories
{
    public class EventRepository : Repository<Event>, IEventRepository
    {
        public EventRepository(EventsAreasDbContext context) 
            : base(context)
        {
        }

        public async Task<IEnumerable<Event>> GetEventsByLocationAsync(int locationId)
        {
            return await _entities
                .Where(e => e.LocationId == locationId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetEventsByCategoryAsync(int categoryId)
        {
            return await _entities
                .Where(e => e.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetEventsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _entities
                .Where(e => e.StartDate >= startDate && e.EndDate <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetEventsByStatusAsync(EventStatus status)
        {
            return await _entities
                .Where(e => e.Status == status)
                .ToListAsync();
        }

        public async Task<Event> GetEventWithDetailsAsync(int id)
        {
            return await _entities
                .Include(e => e.Category)
                .Include(e => e.Location)
                .Include(e => e.Sessions)
                .Include(e => e.EventParticipants)
                    .ThenInclude(ep => ep.Participant)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<(IEnumerable<Event> Events, int TotalCount)> GetFilteredEventsAsync(
            DateTime? startDate,
            DateTime? endDate,
            int? categoryId,
            int? locationId,
            EventStatus? status,
            string searchTerm,
            int pageIndex,
            int pageSize)
        {
            IQueryable<Event> query = _entities
                .Include(e => e.Category)
                .Include(e => e.Location)
                .AsNoTracking();

            // Appliquer les filtres
            if (startDate.HasValue)
                query = query.Where(e => e.StartDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(e => e.EndDate <= endDate.Value);

            if (categoryId.HasValue)
                query = query.Where(e => e.CategoryId == categoryId.Value);

            if (locationId.HasValue)
                query = query.Where(e => e.LocationId == locationId.Value);

            if (status.HasValue)
                query = query.Where(e => e.Status == status.Value);

            if (!string.IsNullOrWhiteSpace(searchTerm))
                query = query.Where(e => e.Title.Contains(searchTerm) || 
                                        e.Description.Contains(searchTerm) ||
                                        e.Category.Name.Contains(searchTerm) ||
                                        e.Location.Name.Contains(searchTerm));

            // Obtenir le nombre total d'éléments
            int totalCount = await query.CountAsync();

            // Appliquer la pagination
            var events = await query
                .OrderByDescending(e => e.StartDate)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (events, totalCount);
        }
    }
}