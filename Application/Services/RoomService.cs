using AutoMapper;
using ISITECH__EventsArea.Domain.Entities;
using ISITECH__EventsArea.Domain.Interfaces;
using ISITECH__EventsArea.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISITECH__EventsArea.Application.Services
{
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RoomService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Room> GetRoomEntityByIdAsync(int id)
        {
            return await _unitOfWork.Rooms.GetRoomWithDetailsAsync(id);
        }

        public async Task<Room> GetRoomByIdAsync(int id)
        {
            return await _unitOfWork.Rooms.GetRoomWithDetailsAsync(id);
        }

        public async Task<IEnumerable<Room>> GetAllRoomsAsync()
        {
            return await _unitOfWork.Rooms.GetAllAsync();
        }

        public async Task<IEnumerable<Room>> GetRoomsByLocationAsync(int locationId)
        {
            var location = await _unitOfWork.Locations.GetByIdAsync(locationId);
            if (location == null)
            {
                throw new KeyNotFoundException($"Location with ID {locationId} not found.");
            }

            return await _unitOfWork.Rooms.GetRoomsByLocationAsync(locationId);
        }

        public async Task<IEnumerable<Room>> GetRoomsByCapacityAsync(int minCapacity)
        {
            if (minCapacity <= 0)
            {
                throw new ArgumentException("Minimum capacity must be greater than zero.");
            }

            return await _unitOfWork.Rooms.GetRoomsByCapacityAsync(minCapacity);
        }

        public async Task<(IEnumerable<Room> Rooms, int TotalCount)> GetFilteredRoomsAsync(
            string searchTerm,
            int? locationId,
            int? minCapacity,
            int pageIndex,
            int pageSize)
        {
            return await _unitOfWork.Rooms.GetFilteredRoomsAsync(
                searchTerm,
                locationId,
                minCapacity,
                pageIndex,
                pageSize);
        }

        public async Task<Room> CreateRoomAsync(Room room)
        {
            // Validate room data
            if (string.IsNullOrWhiteSpace(room.Name))
            {
                throw new ArgumentException("Room name is required.");
            }

            if (room.Capacity <= 0)
            {
                throw new ArgumentException("Room capacity must be greater than zero.");
            }

            // Validate that the location exists
            var location = await _unitOfWork.Locations.GetByIdAsync(room.LocationId);
            if (location == null)
            {
                throw new KeyNotFoundException($"Location with ID {room.LocationId} not found.");
            }

            // Check if a room with the same name already exists in this location
            var existingRoom = await _unitOfWork.Rooms.SingleOrDefaultAsync(
                r => r.Name == room.Name && r.LocationId == room.LocationId);
                
            if (existingRoom != null)
            {
                throw new ArgumentException($"A room with name '{room.Name}' already exists at this location.");
            }

            await _unitOfWork.Rooms.AddAsync(room);
            await _unitOfWork.CompleteAsync();
            
            return room;
        }

        public async Task UpdateRoomAsync(Room room)
        {
            var existingRoom = await _unitOfWork.Rooms.GetByIdAsync(room.Id);
            if (existingRoom == null)
            {
                throw new KeyNotFoundException($"Room with ID {room.Id} not found.");
            }

            // Validate room data
            if (string.IsNullOrWhiteSpace(room.Name))
            {
                throw new ArgumentException("Room name is required.");
            }

            if (room.Capacity <= 0)
            {
                throw new ArgumentException("Room capacity must be greater than zero.");
            }

            // Validate that the location exists
            var location = await _unitOfWork.Locations.GetByIdAsync(room.LocationId);
            if (location == null)
            {
                throw new KeyNotFoundException($"Location with ID {room.LocationId} not found.");
            }

            // Check if another room with the same name already exists in this location
            var duplicateRoom = await _unitOfWork.Rooms.SingleOrDefaultAsync(
                r => r.Name == room.Name && 
                     r.LocationId == room.LocationId && 
                     r.Id != room.Id);
                
            if (duplicateRoom != null)
            {
                throw new ArgumentException($"Another room with name '{room.Name}' already exists at this location.");
            }

            _unitOfWork.Rooms.Update(room);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteRoomAsync(int id)
        {
            var room = await _unitOfWork.Rooms.GetRoomWithDetailsAsync(id);
            if (room == null)
            {
                throw new KeyNotFoundException($"Room with ID {id} not found.");
            }

            // Check if the room has any sessions
            if (room.Sessions.Count > 0)
            {
                throw new InvalidOperationException($"Cannot delete room with ID {id} because it has associated sessions.");
            }

            _unitOfWork.Rooms.Remove(room);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<bool> RoomExistsAsync(int id)
        {
            return await _unitOfWork.Rooms.ExistsAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Session>> GetRoomSessionsAsync(int roomId)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(roomId);
            if (room == null)
            {
                throw new KeyNotFoundException($"Room with ID {roomId} not found.");
            }

            return await _unitOfWork.Sessions.FindAsync(s => s.RoomId == roomId);
        }

        public async Task<bool> IsRoomAvailableAsync(int roomId, DateTime startTime, DateTime endTime)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(roomId);
            if (room == null)
            {
                throw new KeyNotFoundException($"Room with ID {roomId} not found.");
            }

            if (endTime <= startTime)
            {
                throw new ArgumentException("End time must be after start time.");
            }

            // Get all sessions in this room that overlap with the specified time range
            var overlappingSessions = await _unitOfWork.Sessions.FindAsync(s => 
                s.RoomId == roomId && 
                s.StartTime < endTime && 
                s.EndTime > startTime);

            return !overlappingSessions.Any();
        }
    }
}