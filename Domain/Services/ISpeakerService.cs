using ISITECH__EventsArea.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISITECH__EventsArea.Domain.Services
{
    public interface ISpeakerService
    {
        Task<Speaker> GetSpeakerEntityByIdAsync(int id);
        Task<Speaker> GetSpeakerByIdAsync(int id);
        Task<IEnumerable<Speaker>> GetAllSpeakersAsync();
        Task<IEnumerable<Speaker>> GetSpeakersBySessionAsync(int sessionId);
        Task<IEnumerable<Speaker>> GetSpeakersByCompanyAsync(string company);
        Task<(IEnumerable<Speaker> Speakers, int TotalCount)> GetFilteredSpeakersAsync(
            string searchTerm,
            string company,
            int pageIndex,
            int pageSize);
        Task<Speaker> CreateSpeakerAsync(Speaker speaker);
        Task UpdateSpeakerAsync(Speaker speaker);
        Task DeleteSpeakerAsync(int id);
        Task<bool> SpeakerExistsAsync(int id);
        Task<IEnumerable<Session>> GetSpeakerSessionsAsync(int speakerId);
    }
}