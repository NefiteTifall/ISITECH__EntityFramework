using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ISITECH__EventsArea.Domain.Entities;

namespace ISITECH__EventsArea.Domain.Interfaces
{
	public interface IEventRepository : IRepository<Event>
	{
		Task<IEnumerable<Event>> GetEventsByLocationAsync(int locationId);
		Task<IEnumerable<Event>> GetEventsByCategoryAsync(int categoryId);
		Task<IEnumerable<Event>> GetEventsByDateRangeAsync(DateTime startDate, DateTime endDate);
		Task<IEnumerable<Event>> GetEventsByStatusAsync(EventStatus status);
		Task<Event> GetEventWithDetailsAsync(int id);
		Task<(IEnumerable<Event> Events, int TotalCount)> GetFilteredEventsAsync(
			DateTime? startDate, 
			DateTime? endDate, 
			int? categoryId, 
			int? locationId, 
			EventStatus? status, 
			string searchTerm,
			int pageIndex, 
			int pageSize);
	}
}