using ISITECH__EventsArea.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISITECH__EventsArea.Domain.Services
{
    public interface IRoomService
    {
        Task<Room> GetRoomEntityByIdAsync(int id);
        Task<Room> GetRoomByIdAsync(int id);
        Task<IEnumerable<Room>> GetAllRoomsAsync();
        Task<IEnumerable<Room>> GetRoomsByLocationAsync(int locationId);
        Task<IEnumerable<Room>> GetRoomsByCapacityAsync(int minCapacity);
        Task<(IEnumerable<Room> Rooms, int TotalCount)> GetFilteredRoomsAsync(
            string searchTerm,
            int? locationId,
            int? minCapacity,
            int pageIndex,
            int pageSize);
        Task<Room> CreateRoomAsync(Room room);
        Task UpdateRoomAsync(Room room);
        Task DeleteRoomAsync(int id);
        Task<bool> RoomExistsAsync(int id);
        Task<IEnumerable<Session>> GetRoomSessionsAsync(int roomId);
        Task<bool> IsRoomAvailableAsync(int roomId, System.DateTime startTime, System.DateTime endTime);
    }
}