using AutoMapper;
using ISITECH__EventsArea.Domain.Entities;
using ISITECH__EventsArea.Domain.Interfaces;
using ISITECH__EventsArea.Domain.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISITECH__EventsArea.Application.Services
{
    public class LocationService : ILocationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LocationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Location> GetLocationEntityByIdAsync(int id)
        {
            return await _unitOfWork.Locations.GetLocationWithDetailsAsync(id);
        }

        public async Task<Location> GetLocationByIdAsync(int id)
        {
            return await _unitOfWork.Locations.GetLocationWithDetailsAsync(id);
        }

        public async Task<IEnumerable<Location>> GetAllLocationsAsync()
        {
            return await _unitOfWork.Locations.GetAllAsync();
        }

        public async Task<IEnumerable<Location>> GetLocationsByCountryAsync(string country)
        {
            if (string.IsNullOrWhiteSpace(country))
            {
                throw new ArgumentException("Country name is required.");
            }

            return await _unitOfWork.Locations.GetLocationsByCountryAsync(country);
        }

        public async Task<IEnumerable<Location>> GetLocationsByCityAsync(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                throw new ArgumentException("City name is required.");
            }

            return await _unitOfWork.Locations.GetLocationsByCityAsync(city);
        }

        public async Task<(IEnumerable<Location> Locations, int TotalCount)> GetFilteredLocationsAsync(
            string searchTerm,
            string city,
            string country,
            int? minCapacity,
            int pageIndex,
            int pageSize)
        {
            return await _unitOfWork.Locations.GetFilteredLocationsAsync(
                searchTerm,
                city,
                country,
                minCapacity,
                pageIndex,
                pageSize);
        }

        public async Task<Location> CreateLocationAsync(Location location)
        {
            // Validate location data
            if (string.IsNullOrWhiteSpace(location.Name))
            {
                throw new ArgumentException("Location name is required.");
            }

            if (string.IsNullOrWhiteSpace(location.Address))
            {
                throw new ArgumentException("Location address is required.");
            }

            if (string.IsNullOrWhiteSpace(location.City))
            {
                throw new ArgumentException("Location city is required.");
            }

            if (string.IsNullOrWhiteSpace(location.Country))
            {
                throw new ArgumentException("Location country is required.");
            }

            if (location.Capacity <= 0)
            {
                throw new ArgumentException("Location capacity must be greater than zero.");
            }

            // Check if a location with the same name and address already exists
            var existingLocation = await _unitOfWork.Locations.SingleOrDefaultAsync(
                l => l.Name == location.Name && l.Address == location.Address);
                
            if (existingLocation != null)
            {
                throw new ArgumentException($"A location with name '{location.Name}' and address '{location.Address}' already exists.");
            }

            await _unitOfWork.Locations.AddAsync(location);
            await _unitOfWork.CompleteAsync();
            
            return location;
        }

        public async Task UpdateLocationAsync(Location location)
        {
            var existingLocation = await _unitOfWork.Locations.GetByIdAsync(location.Id);
            if (existingLocation == null)
            {
                throw new KeyNotFoundException($"Location with ID {location.Id} not found.");
            }

            // Validate location data
            if (string.IsNullOrWhiteSpace(location.Name))
            {
                throw new ArgumentException("Location name is required.");
            }

            if (string.IsNullOrWhiteSpace(location.Address))
            {
                throw new ArgumentException("Location address is required.");
            }

            if (string.IsNullOrWhiteSpace(location.City))
            {
                throw new ArgumentException("Location city is required.");
            }

            if (string.IsNullOrWhiteSpace(location.Country))
            {
                throw new ArgumentException("Location country is required.");
            }

            if (location.Capacity <= 0)
            {
                throw new ArgumentException("Location capacity must be greater than zero.");
            }

            // Check if another location with the same name and address already exists
            var duplicateLocation = await _unitOfWork.Locations.SingleOrDefaultAsync(
                l => l.Name == location.Name && 
                     l.Address == location.Address && 
                     l.Id != location.Id);
                
            if (duplicateLocation != null)
            {
                throw new ArgumentException($"Another location with name '{location.Name}' and address '{location.Address}' already exists.");
            }

            _unitOfWork.Locations.Update(location);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteLocationAsync(int id)
        {
            var location = await _unitOfWork.Locations.GetLocationWithDetailsAsync(id);
            if (location == null)
            {
                throw new KeyNotFoundException($"Location with ID {id} not found.");
            }

            // Check if the location has any events
            if (location.Events.Count > 0)
            {
                throw new InvalidOperationException($"Cannot delete location with ID {id} because it has associated events.");
            }

            // Check if the location has any rooms
            if (location.Rooms.Count > 0)
            {
                throw new InvalidOperationException($"Cannot delete location with ID {id} because it has associated rooms. Delete the rooms first.");
            }

            _unitOfWork.Locations.Remove(location);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<bool> LocationExistsAsync(int id)
        {
            return await _unitOfWork.Locations.ExistsAsync(l => l.Id == id);
        }

        public async Task<IEnumerable<Event>> GetLocationEventsAsync(int locationId)
        {
            var location = await _unitOfWork.Locations.GetByIdAsync(locationId);
            if (location == null)
            {
                throw new KeyNotFoundException($"Location with ID {locationId} not found.");
            }

            return await _unitOfWork.Events.GetEventsByLocationAsync(locationId);
        }

        public async Task<IEnumerable<Room>> GetLocationRoomsAsync(int locationId)
        {
            var location = await _unitOfWork.Locations.GetByIdAsync(locationId);
            if (location == null)
            {
                throw new KeyNotFoundException($"Location with ID {locationId} not found.");
            }

            return await _unitOfWork.Rooms.GetRoomsByLocationAsync(locationId);
        }
    }
}