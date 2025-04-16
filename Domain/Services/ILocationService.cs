using ISITECH__EventsArea.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISITECH__EventsArea.Domain.Services
{
    public interface ILocationService
    {
        Task<Location> GetLocationEntityByIdAsync(int id);
        Task<Location> GetLocationByIdAsync(int id);
        Task<IEnumerable<Location>> GetAllLocationsAsync();
        Task<IEnumerable<Location>> GetLocationsByCountryAsync(string country);
        Task<IEnumerable<Location>> GetLocationsByCityAsync(string city);
        Task<(IEnumerable<Location> Locations, int TotalCount)> GetFilteredLocationsAsync(
            string searchTerm,
            string city,
            string country,
            int? minCapacity,
            int pageIndex,
            int pageSize);
        Task<Location> CreateLocationAsync(Location location);
        Task UpdateLocationAsync(Location location);
        Task DeleteLocationAsync(int id);
        Task<bool> LocationExistsAsync(int id);
        Task<IEnumerable<Event>> GetLocationEventsAsync(int locationId);
        Task<IEnumerable<Room>> GetLocationRoomsAsync(int locationId);
    }
}