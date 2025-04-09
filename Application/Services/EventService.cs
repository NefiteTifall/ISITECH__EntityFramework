using ISITECH__EventsArea.Domain.Entities;
using ISITECH__EventsArea.Domain.Interfaces;
using ISITECH__EventsArea.Domain.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISITECH__EventsArea.Services
{
    public class EventService : IEventService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EventService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Event> GetEventByIdAsync(int id)
        {
            return await _unitOfWork.Events.GetEventWithDetailsAsync(id);
        }

        public async Task<IEnumerable<Event>> GetAllEventsAsync()
        {
            return await _unitOfWork.Events.GetAllAsync();
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
            return await _unitOfWork.Events.GetFilteredEventsAsync(
                startDate, endDate, categoryId, locationId, status, 
                searchTerm, pageIndex, pageSize);
        }

        public async Task<Event> CreateEventAsync(Event eventEntity)
        {
            if (eventEntity.EndDate <= eventEntity.StartDate)
            {
                throw new ArgumentException("La date de fin doit être postérieure à la date de début");
            }

            await _unitOfWork.Events.AddAsync(eventEntity);
            await _unitOfWork.CompleteAsync();
            return eventEntity;
        }

        public async Task UpdateEventAsync(Event eventEntity)
        {
            // Logique métier similaire à CreateEventAsync
            if (eventEntity.EndDate <= eventEntity.StartDate)
            {
                throw new ArgumentException("La date de fin doit être postérieure à la date de début");
            }

            _unitOfWork.Events.Update(eventEntity);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteEventAsync(int id)
        {
            var eventEntity = await _unitOfWork.Events.GetByIdAsync(id);
            if (eventEntity != null)
            {
                _unitOfWork.Events.Remove(eventEntity);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task<bool> EventExistsAsync(int id)
        {
            return await _unitOfWork.Events.ExistsAsync(e => e.Id == id);
        }
    }
}