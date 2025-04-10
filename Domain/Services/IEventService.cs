// Domain/Services/IEventService.cs
using ISITECH__EventsArea.API.DTO;
using ISITECH__EventsArea.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISITECH__EventsArea.Domain.Services
{
	public interface IEventService
	{
		Task<Event> GetEventEntityByIdAsync(int id);
		Task<EventDto> GetEventByIdAsync(int id);
		Task<IEnumerable<EventDto>> GetAllEventsAsync();
		Task<(IEnumerable<EventDto> Events, int TotalCount)> GetFilteredEventsAsync(
			DateTime? startDate, 
			DateTime? endDate, 
			int? categoryId, 
			int? locationId, 
			EventStatus? status, 
			string? searchTerm,
			int pageIndex, 
			int pageSize);
		Task<EventDto> CreateEventAsync(EventCreateDto eventDto);
		Task PatchEventAsync(int id, EventPatchDto eventDto);
		Task DeleteEventAsync(int id);
		Task<bool> EventExistsAsync(int id);
	}
}