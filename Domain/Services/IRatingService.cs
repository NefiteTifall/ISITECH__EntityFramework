using ISITECH__EventsArea.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISITECH__EventsArea.Domain.Services
{
    public interface IRatingService
    {
        Task<Rating> GetRatingEntityByIdAsync(int id);
        Task<Rating> GetRatingByIdAsync(int id);
        Task<IEnumerable<Rating>> GetAllRatingsAsync();
        Task<IEnumerable<Rating>> GetRatingsBySessionAsync(int sessionId);
        Task<IEnumerable<Rating>> GetRatingsByParticipantAsync(int participantId);
        Task<double> GetAverageRatingForSessionAsync(int sessionId);
        Task<(IEnumerable<Rating> Ratings, int TotalCount)> GetFilteredRatingsAsync(
            int? sessionId,
            int? participantId,
            int? minScore,
            int? maxScore,
            int pageIndex,
            int pageSize);
        Task<Rating> CreateRatingAsync(Rating rating);
        Task UpdateRatingAsync(Rating rating);
        Task DeleteRatingAsync(int id);
        Task<bool> RatingExistsAsync(int id);
        Task<Dictionary<int, double>> GetAverageRatingsForEventAsync(int eventId);
    }
}