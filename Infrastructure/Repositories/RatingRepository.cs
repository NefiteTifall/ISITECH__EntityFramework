using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISITECH__EventsArea.Domain.Entities;
using ISITECH__EventsArea.Domain.Interfaces;
using ISITECH__EventsArea.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ISITECH__EventsArea.Infrastructure.Repositories
{
    public class RatingRepository : Repository<Rating>, IRatingRepository
    {
        public RatingRepository(EventsAreasDbContext context) 
            : base(context)
        {
        }

        public async Task<Rating> GetRatingWithDetailsAsync(int id)
        {
            return await _entities
                .Include(r => r.Session)
                .Include(r => r.Participant)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Rating>> GetRatingsBySessionAsync(int sessionId)
        {
            return await _entities
                .Where(r => r.SessionId == sessionId)
                .Include(r => r.Participant)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Rating>> GetRatingsByParticipantAsync(int participantId)
        {
            return await _entities
                .Where(r => r.ParticipantId == participantId)
                .Include(r => r.Session)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<double> GetAverageRatingForSessionAsync(int sessionId)
        {
            var ratings = await _entities
                .Where(r => r.SessionId == sessionId)
                .Select(r => r.Score)
                .ToListAsync();

            if (ratings.Count == 0)
                return 0;

            return ratings.Average();
        }

        public async Task<(IEnumerable<Rating> Ratings, int TotalCount)> GetFilteredRatingsAsync(
            int? sessionId,
            int? participantId,
            int? minScore,
            int? maxScore,
            int pageIndex,
            int pageSize)
        {
            IQueryable<Rating> query = _entities
                .Include(r => r.Session)
                .Include(r => r.Participant)
                .AsNoTracking();

            // Apply filters
            if (sessionId.HasValue)
            {
                query = query.Where(r => r.SessionId == sessionId.Value);
            }

            if (participantId.HasValue)
            {
                query = query.Where(r => r.ParticipantId == participantId.Value);
            }

            if (minScore.HasValue)
            {
                query = query.Where(r => r.Score >= minScore.Value);
            }

            if (maxScore.HasValue)
            {
                query = query.Where(r => r.Score <= maxScore.Value);
            }

            // Get total count
            int totalCount = await query.CountAsync();

            // Apply pagination
            var ratings = await query
                .OrderByDescending(r => r.CreatedAt)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (ratings, totalCount);
        }
    }
}