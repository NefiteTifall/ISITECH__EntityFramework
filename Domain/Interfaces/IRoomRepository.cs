using System.Collections.Generic;
using System.Threading.Tasks;
using ISITECH__EventsArea.Domain.Entities;

namespace ISITECH__EventsArea.Domain.Interfaces
{
	public interface IRoomRepository : IRepository<Room>
	{
		Task<Room> GetRoomWithDetailsAsync(int id);
		Task<IEnumerable<Room>> GetRoomsByLocationAsync(int locationId);
		Task<IEnumerable<Room>> GetRoomsByCapacityAsync(int minCapacity);
		Task<(IEnumerable<Room> Rooms, int TotalCount)> GetFilteredRoomsAsync(
			string searchTerm,
			int? locationId,
			int? minCapacity,
			int pageIndex,
			int pageSize);
	}
}