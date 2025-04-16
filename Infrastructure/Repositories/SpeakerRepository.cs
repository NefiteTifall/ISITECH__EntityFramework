using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISITECH__EventsArea.Domain.Entities;
using ISITECH__EventsArea.Domain.Interfaces;
using ISITECH__EventsArea.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ISITECH__EventsArea.Infrastructure.Repositories
{
    public class SpeakerRepository : Repository<Speaker>, ISpeakerRepository
    {
        public SpeakerRepository(EventsAreasDbContext context) 
            : base(context)
        {
        }

        public async Task<Speaker> GetSpeakerWithDetailsAsync(int id)
        {
            return await _entities
                .Include(s => s.SessionSpeakers)
                    .ThenInclude(ss => ss.Session)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Speaker>> GetSpeakersBySessionAsync(int sessionId)
        {
            return await _entities
                .Where(s => s.SessionSpeakers.Any(ss => ss.SessionId == sessionId))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Speaker>> GetSpeakersByCompanyAsync(string company)
        {
            return await _entities
                .Where(s => s.Company == company)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<(IEnumerable<Speaker> Speakers, int TotalCount)> GetFilteredSpeakersAsync(
            string searchTerm,
            string company,
            int pageIndex,
            int pageSize)
        {
            IQueryable<Speaker> query = _entities.AsNoTracking();

            // Apply filters
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(s => 
                    s.FirstName.Contains(searchTerm) || 
                    s.LastName.Contains(searchTerm) || 
                    s.Bio.Contains(searchTerm) ||
                    s.Email.Contains(searchTerm) ||
                    s.Company.Contains(searchTerm));
            }

            if (!string.IsNullOrWhiteSpace(company))
            {
                query = query.Where(s => s.Company == company);
            }

            // Get total count
            int totalCount = await query.CountAsync();

            // Apply pagination
            var speakers = await query
                .OrderBy(s => s.LastName)
                .ThenBy(s => s.FirstName)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (speakers, totalCount);
        }
    }
}