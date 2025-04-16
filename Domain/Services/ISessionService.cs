using ISITECH__EventsArea.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISITECH__EventsArea.Domain.Services
{
    public interface ISessionService
    {
        Task<Session> GetSessionEntityByIdAsync(int id);
        Task<Session> GetSessionByIdAsync(int id);
        Task<IEnumerable<Session>> GetAllSessionsAsync();
        Task<IEnumerable<Session>> GetSessionsByEventAsync(int eventId);
        Task<IEnumerable<Session>> GetSessionsByRoomAsync(int roomId);
        Task<IEnumerable<Session>> GetSessionsBySpeakerAsync(int speakerId);
        Task<(IEnumerable<Session> Sessions, int TotalCount)> GetFilteredSessionsAsync(
            int? eventId,
            int? roomId,
            int? speakerId,
            string searchTerm,
            int pageIndex,
            int pageSize);
        Task<Session> CreateSessionAsync(Session session);
        Task UpdateSessionAsync(Session session);
        Task DeleteSessionAsync(int id);
        Task<bool> SessionExistsAsync(int id);
        Task AssignSessionToRoomAsync(int sessionId, int roomId);
        Task AddSpeakerToSessionAsync(int sessionId, int speakerId, string role);
        Task RemoveSpeakerFromSessionAsync(int sessionId, int speakerId);
    }
}