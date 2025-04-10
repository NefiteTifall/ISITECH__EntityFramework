// Application/Services/EventService.cs
using AutoMapper;
using ISITECH__EventsArea.API.DTO;
using ISITECH__EventsArea.Domain.Entities;
using ISITECH__EventsArea.Domain.Interfaces;
using ISITECH__EventsArea.Domain.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISITECH__EventsArea.Application.Services
{
    public class EventService : IEventService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EventService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Event> GetEventEntityByIdAsync(int id)
        {
            return await _unitOfWork.Events.GetEventWithDetailsAsync(id);
        }

        public async Task<EventDto> GetEventByIdAsync(int id)
        {
            var eventEntity = await _unitOfWork.Events.GetEventWithDetailsAsync(id);
            return _mapper.Map<EventDto>(eventEntity);
        }

        public async Task<IEnumerable<EventDto>> GetAllEventsAsync()
        {
            var events = await _unitOfWork.Events.GetAllAsync();
            return _mapper.Map<IEnumerable<EventDto>>(events);
        }

        public async Task<(IEnumerable<EventDto> Events, int TotalCount)> GetFilteredEventsAsync(
            DateTime? startDate, 
            DateTime? endDate, 
            int? categoryId, 
            int? locationId, 
            EventStatus? status, 
            string searchTerm,
            int pageIndex, 
            int pageSize)
        {
            var result = await _unitOfWork.Events.GetFilteredEventsAsync(
                startDate, endDate, categoryId, locationId, status, 
                searchTerm, pageIndex, pageSize);
                
            var dtos = _mapper.Map<IEnumerable<EventDto>>(result.Events);
            return (dtos, result.TotalCount);
        }

        public async Task<EventDto> CreateEventAsync(EventCreateDto eventDto)
        {
            if (eventDto.EndDate <= eventDto.StartDate)
            {
                throw new ArgumentException("La date de fin doit être postérieure à la date de début");
            }

            var eventEntity = _mapper.Map<Event>(eventDto);
            await _unitOfWork.Events.AddAsync(eventEntity);
            await _unitOfWork.CompleteAsync();
            
            // Recharger l'entité complète avec les relations
            var createdEvent = await _unitOfWork.Events.GetEventWithDetailsAsync(eventEntity.Id);
            return _mapper.Map<EventDto>(createdEvent);
        }

        public async Task PatchEventAsync(int id, EventPatchDto patchDto)
        {
            var existingEvent = await _unitOfWork.Events.GetByIdAsync(id);
            if (existingEvent == null)
            {
                throw new KeyNotFoundException($"Événement avec l'ID {id} non trouvé.");
            }

            // Appliquer les modifications seulement si non null
            if (patchDto.Title != null)
                existingEvent.Title = patchDto.Title;
    
            if (patchDto.Description != null)
                existingEvent.Description = patchDto.Description;
    
            if (patchDto.StartDate.HasValue)
                existingEvent.StartDate = patchDto.StartDate.Value;
    
            if (patchDto.EndDate.HasValue)
                existingEvent.EndDate = patchDto.EndDate.Value;
    
            if (patchDto.Status.HasValue)
                existingEvent.Status = patchDto.Status.Value;
    
            if (patchDto.CategoryId.HasValue)
                existingEvent.CategoryId = patchDto.CategoryId.Value;
    
            if (patchDto.LocationId.HasValue)
                existingEvent.LocationId = patchDto.LocationId.Value;

            // Validation métier
            if (existingEvent.EndDate <= existingEvent.StartDate)
            {
                throw new ArgumentException("La date de fin doit être postérieure à la date de début");
            }

            _unitOfWork.Events.Update(existingEvent);
            await _unitOfWork.CompleteAsync();
        }
        
        public async Task UpdateEventEntityAsync(Event eventEntity)
        {
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
            if (eventEntity == null)
            {
                throw new KeyNotFoundException($"Événement avec l'ID {id} non trouvé.");
            }
            
            _unitOfWork.Events.Remove(eventEntity);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<bool> EventExistsAsync(int id)
        {
            return await _unitOfWork.Events.ExistsAsync(e => e.Id == id);
        }
    }
}