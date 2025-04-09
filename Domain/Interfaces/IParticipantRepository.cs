using System.Collections.Generic;
using System.Threading.Tasks;
using ISITECH__EventsArea.Domain.Entities;

namespace ISITECH__EventsArea.Domain.Interfaces
{
	public interface IParticipantRepository : IRepository<Participant>
	{
		Task<IEnumerable<Participant>> GetParticipantsByEventAsync(int eventId);
		Task<Participant> GetParticipantByEmailAsync(string email);
		Task<IEnumerable<Event>> GetParticipantEventsHistoryAsync(int participantId);
		Task<bool> IsParticipantRegisteredForEventAsync(int participantId, int eventId);
		Task<EventParticipant> GetEventParticipantAsync(int eventId, int participantId);
		Task AddEventParticipantAsync(EventParticipant eventParticipant);
		void RemoveEventParticipant(EventParticipant eventParticipant);
		Task<(IEnumerable<Participant> Participants, int TotalCount)> GetParticipantsByEventPaginatedAsync(
			int eventId, int pageIndex, int pageSize, string searchTerm = null);
	}
}