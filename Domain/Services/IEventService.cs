using ISITECH__EventsArea.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISITECH__EventsArea.Domain.Services
{
	public interface IEventService
	{
		Task<Event> GetEventByIdAsync(int id);
		Task<IEnumerable<Event>> GetAllEventsAsync();
		Task<(IEnumerable<Event> Events, int TotalCount)> GetFilteredEventsAsync(
			DateTime? startDate, 
			DateTime? endDate, 
			int? categoryId, 
			int? locationId, 
			EventStatus? status, 
			string searchTerm,
			int pageIndex, 
			int pageSize);
		Task<Event> CreateEventAsync(Event eventEntity);
		Task UpdateEventAsync(Event eventEntity);
		Task DeleteEventAsync(int id);
		Task<bool> EventExistsAsync(int id);
	}
}