using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISITECH__EventsArea.Domain.Entities;
using ISITECH__EventsArea.Domain.Interfaces;
using ISITECH__EventsArea.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ISITECH__EventsArea.Infrastructure.Repositories
{
    public class RoomRepository : Repository<Room>, IRoomRepository
    {
        public RoomRepository(EventsAreasDbContext context) 
            : base(context)
        {
        }

        public async Task<Room> GetRoomWithDetailsAsync(int id)
        {
            return await _entities
                .Include(r => r.Location)
                .Include(r => r.Sessions)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Room>> GetRoomsByLocationAsync(int locationId)
        {
            return await _entities
                .Where(r => r.LocationId == locationId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Room>> GetRoomsByCapacityAsync(int minCapacity)
        {
            return await _entities
                .Where(r => r.Capacity >= minCapacity)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<(IEnumerable<Room> Rooms, int TotalCount)> GetFilteredRoomsAsync(
            string searchTerm,
            int? locationId,
            int? minCapacity,
            int pageIndex,
            int pageSize)
        {
            IQueryable<Room> query = _entities
                .Include(r => r.Location)
                .AsNoTracking();

            // Apply filters
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(r => 
                    r.Name.Contains(searchTerm) || 
                    r.Location.Name.Contains(searchTerm));
            }

            if (locationId.HasValue)
            {
                query = query.Where(r => r.LocationId == locationId.Value);
            }

            if (minCapacity.HasValue)
            {
                query = query.Where(r => r.Capacity >= minCapacity.Value);
            }

            // Get total count
            int totalCount = await query.CountAsync();

            // Apply pagination
            var rooms = await query
                .OrderBy(r => r.LocationId)
                .ThenBy(r => r.Name)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (rooms, totalCount);
        }
    }
}