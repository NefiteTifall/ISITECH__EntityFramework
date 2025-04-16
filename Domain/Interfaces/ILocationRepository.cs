using System.Collections.Generic;
using System.Threading.Tasks;
using ISITECH__EventsArea.Domain.Entities;

namespace ISITECH__EventsArea.Domain.Interfaces
{
	public interface ILocationRepository : IRepository<Location>
	{
		Task<Location> GetLocationWithDetailsAsync(int id);
		Task<IEnumerable<Location>> GetLocationsByCountryAsync(string country);
		Task<IEnumerable<Location>> GetLocationsByCityAsync(string city);
		Task<(IEnumerable<Location> Locations, int TotalCount)> GetFilteredLocationsAsync(
			string searchTerm,
			string city,
			string country,
			int? minCapacity,
			int pageIndex,
			int pageSize);
	}
}