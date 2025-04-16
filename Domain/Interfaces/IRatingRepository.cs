using System.Collections.Generic;
using System.Threading.Tasks;
using ISITECH__EventsArea.Domain.Entities;

namespace ISITECH__EventsArea.Domain.Interfaces
{
	public interface IRatingRepository : IRepository<Rating>
	{
		Task<Rating> GetRatingWithDetailsAsync(int id);
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
	}
}