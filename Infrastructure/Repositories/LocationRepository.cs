using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISITECH__EventsArea.Domain.Entities;
using ISITECH__EventsArea.Domain.Interfaces;
using ISITECH__EventsArea.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ISITECH__EventsArea.Infrastructure.Repositories
{
    public class LocationRepository : Repository<Location>, ILocationRepository
    {
        public LocationRepository(EventsAreasDbContext context) 
            : base(context)
        {
        }

        public async Task<Location> GetLocationWithDetailsAsync(int id)
        {
            return await _entities
                .Include(l => l.Rooms)
                .Include(l => l.Events)
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<IEnumerable<Location>> GetLocationsByCountryAsync(string country)
        {
            return await _entities
                .Where(l => l.Country == country)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Location>> GetLocationsByCityAsync(string city)
        {
            return await _entities
                .Where(l => l.City == city)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<(IEnumerable<Location> Locations, int TotalCount)> GetFilteredLocationsAsync(
            string searchTerm,
            string city,
            string country,
            int? minCapacity,
            int pageIndex,
            int pageSize)
        {
            IQueryable<Location> query = _entities.AsNoTracking();

            // Apply filters
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(l => 
                    l.Name.Contains(searchTerm) || 
                    l.Address.Contains(searchTerm) || 
                    l.City.Contains(searchTerm) ||
                    l.Country.Contains(searchTerm));
            }

            if (!string.IsNullOrWhiteSpace(city))
            {
                query = query.Where(l => l.City == city);
            }

            if (!string.IsNullOrWhiteSpace(country))
            {
                query = query.Where(l => l.Country == country);
            }

            if (minCapacity.HasValue)
            {
                query = query.Where(l => l.Capacity >= minCapacity.Value);
            }

            // Get total count
            int totalCount = await query.CountAsync();

            // Apply pagination
            var locations = await query
                .OrderBy(l => l.Country)
                .ThenBy(l => l.City)
                .ThenBy(l => l.Name)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (locations, totalCount);
        }
    }
}