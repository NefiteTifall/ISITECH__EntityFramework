using ISITECH__EventsArea.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISITECH__EventsArea.Domain.Services
{
    public interface IParticipantService
    {
        Task<Participant> GetParticipantEntityByIdAsync(int id);
        Task<Participant> GetParticipantByIdAsync(int id);
        Task<IEnumerable<Participant>> GetAllParticipantsAsync();
        Task<IEnumerable<Participant>> GetParticipantsByEventAsync(int eventId);
        Task<(IEnumerable<Participant> Participants, int TotalCount)> GetFilteredParticipantsAsync(
            string searchTerm,
            string company,
            int pageIndex,
            int pageSize);
        Task<Participant> CreateParticipantAsync(Participant participant);
        Task UpdateParticipantAsync(Participant participant);
        Task DeleteParticipantAsync(int id);
        Task<bool> ParticipantExistsAsync(int id);
        Task RegisterParticipantForEventAsync(int participantId, int eventId);
        Task UnregisterParticipantFromEventAsync(int participantId, int eventId);
        Task<IEnumerable<Event>> GetParticipantEventHistoryAsync(int participantId);
    }
}