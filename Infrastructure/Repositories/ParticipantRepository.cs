using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISITECH__EventsArea.Domain.Entities;
using ISITECH__EventsArea.Domain.Interfaces;
using ISITECH__EventsArea.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ISITECH__EventsArea.Infrastructure.Repositories
{
    public class ParticipantRepository : Repository<Participant>, IParticipantRepository
    {
        public ParticipantRepository(EventsAreasDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<Participant>> GetParticipantsByEventAsync(int eventId)
        {
            return await _context.EventParticipants
                .Where(ep => ep.EventId == eventId)
                .Select(ep => ep.Participant)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Participant> GetParticipantByEmailAsync(string email)
        {
            return await _entities
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Email == email);
        }

        public async Task<IEnumerable<Event>> GetParticipantEventsHistoryAsync(int participantId)
        {
            return await _context.EventParticipants
                .Where(ep => ep.ParticipantId == participantId)
                .Select(ep => ep.Event)
                .Include(e => e.Category)
                .Include(e => e.Location)
                .OrderByDescending(e => e.StartDate)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> IsParticipantRegisteredForEventAsync(int participantId, int eventId)
        {
            return await _context.EventParticipants
                .AnyAsync(ep => ep.ParticipantId == participantId && ep.EventId == eventId);
        }

        public async Task<EventParticipant> GetEventParticipantAsync(int eventId, int participantId)
        {
            return await _context.EventParticipants
                .FirstOrDefaultAsync(ep => ep.EventId == eventId && ep.ParticipantId == participantId);
        }

        public async Task AddEventParticipantAsync(EventParticipant eventParticipant)
        {
            await _context.EventParticipants.AddAsync(eventParticipant);
        }

        public void RemoveEventParticipant(EventParticipant eventParticipant)
        {
            _context.EventParticipants.Remove(eventParticipant);
        }

        public async Task<(IEnumerable<Participant> Participants, int TotalCount)> GetParticipantsByEventPaginatedAsync(
            int eventId, int pageIndex, int pageSize, string searchTerm = null)
        {
            IQueryable<Participant> query = _context.EventParticipants
                .Where(ep => ep.EventId == eventId)
                .Select(ep => ep.Participant)
                .AsNoTracking();

            // Appliquer le filtre de recherche si fourni
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(p => p.FirstName.Contains(searchTerm) ||
                                        p.LastName.Contains(searchTerm) ||
                                        p.Email.Contains(searchTerm) ||
                                        p.Company.Contains(searchTerm));
            }

            // Obtenir le nombre total de participants
            int totalCount = await query.CountAsync();

            // Appliquer la pagination
            var participants = await query
                .OrderBy(p => p.LastName)
                .ThenBy(p => p.FirstName)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (participants, totalCount);
        }
    }
}