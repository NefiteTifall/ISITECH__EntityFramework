using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISITECH__EventsArea.Domain.Entities;
using ISITECH__EventsArea.Domain.Interfaces;
using ISITECH__EventsArea.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ISITECH__EventsArea.Infrastructure.Repositories
{
    public class SessionRepository : Repository<Session>, ISessionRepository
    {
        public SessionRepository(EventsAreasDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<Session>> GetSessionsByEventAsync(int eventId)
        {
            return await _entities
                .Where(s => s.EventId == eventId)
                .Include(s => s.Room)
                .OrderBy(s => s.StartTime)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Session>> GetSessionsByRoomAsync(int roomId)
        {
            return await _entities
                .Where(s => s.RoomId == roomId)
                .Include(s => s.Event)
                .OrderBy(s => s.StartTime)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Session>> GetSessionsBySpeakerAsync(int speakerId)
        {
            return await _context.SessionSpeakers
                .Where(ss => ss.SpeakerId == speakerId)
                .Select(ss => ss.Session)
                .Include(s => s.Event)
                .Include(s => s.Room)
                .OrderBy(s => s.StartTime)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Session>> GetSessionsByTimeSlotAsync(int eventId, DateTime startTime, DateTime endTime)
        {
            return await _entities
                .Where(s => s.EventId == eventId && 
                       ((s.StartTime >= startTime && s.StartTime < endTime) || 
                        (s.EndTime > startTime && s.EndTime <= endTime) ||
                        (s.StartTime <= startTime && s.EndTime >= endTime)))
                .Include(s => s.Room)
                .OrderBy(s => s.StartTime)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Session> GetSessionWithDetailsAsync(int id)
        {
            return await _entities
                .Include(s => s.Event)
                .Include(s => s.Room)
                    .ThenInclude(r => r.Location)
                .Include(s => s.SessionSpeakers)
                    .ThenInclude(ss => ss.Speaker)
                .Include(s => s.Ratings)
                    .ThenInclude(r => r.Participant)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<bool> IsRoomAvailableForSessionAsync(int roomId, DateTime startTime, DateTime endTime, int? excludeSessionId = null)
        {
            // Vérifier s'il y a des sessions qui se chevauchent dans la même salle
            var query = _entities
                .Where(s => s.RoomId == roomId &&
                       ((s.StartTime >= startTime && s.StartTime < endTime) || 
                        (s.EndTime > startTime && s.EndTime <= endTime) ||
                        (s.StartTime <= startTime && s.EndTime >= endTime)));

            // Exclure la session en cours de modification si spécifiée
            if (excludeSessionId.HasValue)
            {
                query = query.Where(s => s.Id != excludeSessionId.Value);
            }

            // La salle est disponible s'il n'y a pas de sessions qui se chevauchent
            return !await query.AnyAsync();
        }

        public async Task<double> GetSessionAverageRatingAsync(int sessionId)
        {
            var ratings = await _context.Ratings
                .Where(r => r.SessionId == sessionId)
                .ToListAsync();

            if (!ratings.Any())
                return 0;

            return ratings.Average(r => r.Score);
        }

        public async Task AddSessionSpeakerAsync(SessionSpeaker sessionSpeaker)
        {
            await _context.SessionSpeakers.AddAsync(sessionSpeaker);
        }

        public void RemoveSessionSpeaker(SessionSpeaker sessionSpeaker)
        {
            _context.SessionSpeakers.Remove(sessionSpeaker);
        }

        public async Task<SessionSpeaker> GetSessionSpeakerAsync(int sessionId, int speakerId)
        {
            return await _context.SessionSpeakers
                .FirstOrDefaultAsync(ss => ss.SessionId == sessionId && ss.SpeakerId == speakerId);
        }
    }
}