using AutoMapper;
using ISITECH__EventsArea.Domain.Entities;
using ISITECH__EventsArea.Domain.Interfaces;
using ISITECH__EventsArea.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISITECH__EventsArea.Application.Services
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Session> GetSessionEntityByIdAsync(int id)
        {
            return await _unitOfWork.Sessions.GetByIdAsync(id);
        }

        public async Task<Session> GetSessionByIdAsync(int id)
        {
            return await _unitOfWork.Sessions.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Session>> GetAllSessionsAsync()
        {
            return await _unitOfWork.Sessions.GetAllAsync();
        }

        public async Task<IEnumerable<Session>> GetSessionsByEventAsync(int eventId)
        {
            var eventEntity = await _unitOfWork.Events.GetByIdAsync(eventId);
            if (eventEntity == null)
            {
                throw new KeyNotFoundException($"Event with ID {eventId} not found.");
            }

            return await _unitOfWork.Sessions.FindAsync(s => s.EventId == eventId);
        }

        public async Task<IEnumerable<Session>> GetSessionsByRoomAsync(int roomId)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(roomId);
            if (room == null)
            {
                throw new KeyNotFoundException($"Room with ID {roomId} not found.");
            }

            return await _unitOfWork.Sessions.FindAsync(s => s.RoomId == roomId);
        }

        public async Task<IEnumerable<Session>> GetSessionsBySpeakerAsync(int speakerId)
        {
            var speaker = await _unitOfWork.Speakers.GetByIdAsync(speakerId);
            if (speaker == null)
            {
                throw new KeyNotFoundException($"Speaker with ID {speakerId} not found.");
            }

            // Get all sessions where this speaker is assigned
            var sessions = await _unitOfWork.Sessions.FindAsync(s => 
                s.SessionSpeakers.Any(ss => ss.SpeakerId == speakerId));

            return sessions;
        }

        public async Task<(IEnumerable<Session> Sessions, int TotalCount)> GetFilteredSessionsAsync(
            int? eventId,
            int? roomId,
            int? speakerId,
            string searchTerm,
            int pageIndex,
            int pageSize)
        {
            // Implement filtering logic using the repository
            var query = _unitOfWork.Sessions.GetAllAsync().Result.AsQueryable();

            // Apply filters
            if (eventId.HasValue)
            {
                query = query.Where(s => s.EventId == eventId.Value);
            }

            if (roomId.HasValue)
            {
                query = query.Where(s => s.RoomId == roomId.Value);
            }

            if (speakerId.HasValue)
            {
                query = query.Where(s => s.SessionSpeakers.Any(ss => ss.SpeakerId == speakerId.Value));
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(s => 
                    s.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || 
                    s.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            // Get total count
            int totalCount = query.Count();

            // Apply pagination
            var sessions = query
                .OrderBy(s => s.StartTime)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return (sessions, totalCount);
        }

        public async Task<Session> CreateSessionAsync(Session session)
        {
            // Validate session data
            if (string.IsNullOrWhiteSpace(session.Title))
            {
                throw new ArgumentException("Session title is required.");
            }

            if (session.EndTime <= session.StartTime)
            {
                throw new ArgumentException("Session end time must be after start time.");
            }

            // Validate that the event exists
            var eventEntity = await _unitOfWork.Events.GetByIdAsync(session.EventId);
            if (eventEntity == null)
            {
                throw new KeyNotFoundException($"Event with ID {session.EventId} not found.");
            }

            // Validate that the session is within the event's time range
            if (session.StartTime < eventEntity.StartDate || session.EndTime > eventEntity.EndDate)
            {
                throw new ArgumentException("Session must be scheduled within the event's time range.");
            }

            // If a room is assigned, validate that it exists and is available
            if (session.RoomId.HasValue)
            {
                var room = await _unitOfWork.Rooms.GetByIdAsync(session.RoomId.Value);
                if (room == null)
                {
                    throw new KeyNotFoundException($"Room with ID {session.RoomId.Value} not found.");
                }

                // Check if the room is available during the session time
                var isAvailable = await IsRoomAvailableAsync(session.RoomId.Value, session.StartTime, session.EndTime, null);
                if (!isAvailable)
                {
                    throw new InvalidOperationException($"Room with ID {session.RoomId.Value} is not available during the specified time.");
                }
            }

            await _unitOfWork.Sessions.AddAsync(session);
            await _unitOfWork.CompleteAsync();
            
            return session;
        }

        public async Task UpdateSessionAsync(Session session)
        {
            var existingSession = await _unitOfWork.Sessions.GetByIdAsync(session.Id);
            if (existingSession == null)
            {
                throw new KeyNotFoundException($"Session with ID {session.Id} not found.");
            }

            // Validate session data
            if (string.IsNullOrWhiteSpace(session.Title))
            {
                throw new ArgumentException("Session title is required.");
            }

            if (session.EndTime <= session.StartTime)
            {
                throw new ArgumentException("Session end time must be after start time.");
            }

            // Validate that the event exists
            var eventEntity = await _unitOfWork.Events.GetByIdAsync(session.EventId);
            if (eventEntity == null)
            {
                throw new KeyNotFoundException($"Event with ID {session.EventId} not found.");
            }

            // Validate that the session is within the event's time range
            if (session.StartTime < eventEntity.StartDate || session.EndTime > eventEntity.EndDate)
            {
                throw new ArgumentException("Session must be scheduled within the event's time range.");
            }

            // If a room is assigned, validate that it exists and is available
            if (session.RoomId.HasValue)
            {
                var room = await _unitOfWork.Rooms.GetByIdAsync(session.RoomId.Value);
                if (room == null)
                {
                    throw new KeyNotFoundException($"Room with ID {session.RoomId.Value} not found.");
                }

                // Check if the room is available during the session time (excluding this session)
                var isAvailable = await IsRoomAvailableAsync(session.RoomId.Value, session.StartTime, session.EndTime, session.Id);
                if (!isAvailable)
                {
                    throw new InvalidOperationException($"Room with ID {session.RoomId.Value} is not available during the specified time.");
                }
            }

            _unitOfWork.Sessions.Update(session);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteSessionAsync(int id)
        {
            var session = await _unitOfWork.Sessions.GetByIdAsync(id);
            if (session == null)
            {
                throw new KeyNotFoundException($"Session with ID {id} not found.");
            }

            _unitOfWork.Sessions.Remove(session);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<bool> SessionExistsAsync(int id)
        {
            return await _unitOfWork.Sessions.ExistsAsync(s => s.Id == id);
        }

        public async Task AssignSessionToRoomAsync(int sessionId, int roomId)
        {
            var session = await _unitOfWork.Sessions.GetByIdAsync(sessionId);
            if (session == null)
            {
                throw new KeyNotFoundException($"Session with ID {sessionId} not found.");
            }

            var room = await _unitOfWork.Rooms.GetByIdAsync(roomId);
            if (room == null)
            {
                throw new KeyNotFoundException($"Room with ID {roomId} not found.");
            }

            // Check if the room is available during the session time
            var isAvailable = await IsRoomAvailableAsync(roomId, session.StartTime, session.EndTime, sessionId);
            if (!isAvailable)
            {
                throw new InvalidOperationException($"Room with ID {roomId} is not available during the session time.");
            }

            session.RoomId = roomId;
            _unitOfWork.Sessions.Update(session);
            await _unitOfWork.CompleteAsync();
        }

        public async Task AddSpeakerToSessionAsync(int sessionId, int speakerId, string role)
        {
            var session = await _unitOfWork.Sessions.GetByIdAsync(sessionId);
            if (session == null)
            {
                throw new KeyNotFoundException($"Session with ID {sessionId} not found.");
            }

            var speaker = await _unitOfWork.Speakers.GetByIdAsync(speakerId);
            if (speaker == null)
            {
                throw new KeyNotFoundException($"Speaker with ID {speakerId} not found.");
            }

            // Check if the speaker is already assigned to this session
            var isAssigned = await _unitOfWork.Sessions.ExistsAsync(s => 
                s.Id == sessionId && 
                s.SessionSpeakers.Any(ss => ss.SpeakerId == speakerId));

            if (isAssigned)
            {
                throw new InvalidOperationException($"Speaker with ID {speakerId} is already assigned to session with ID {sessionId}.");
            }

            var sessionSpeaker = new SessionSpeaker
            {
                SessionId = sessionId,
                SpeakerId = speakerId,
                Role = role ?? "Speaker" // Default role if not provided
            };

            // Add the speaker to the session
            session.SessionSpeakers.Add(sessionSpeaker);
            _unitOfWork.Sessions.Update(session);
            await _unitOfWork.CompleteAsync();
        }

        public async Task RemoveSpeakerFromSessionAsync(int sessionId, int speakerId)
        {
            var session = await _unitOfWork.Sessions.GetByIdAsync(sessionId);
            if (session == null)
            {
                throw new KeyNotFoundException($"Session with ID {sessionId} not found.");
            }

            var speaker = await _unitOfWork.Speakers.GetByIdAsync(speakerId);
            if (speaker == null)
            {
                throw new KeyNotFoundException($"Speaker with ID {speakerId} not found.");
            }

            // Find the session-speaker relationship
            var sessionSpeaker = session.SessionSpeakers.FirstOrDefault(ss => ss.SpeakerId == speakerId);
            if (sessionSpeaker == null)
            {
                throw new InvalidOperationException($"Speaker with ID {speakerId} is not assigned to session with ID {sessionId}.");
            }

            // Remove the speaker from the session
            session.SessionSpeakers.Remove(sessionSpeaker);
            _unitOfWork.Sessions.Update(session);
            await _unitOfWork.CompleteAsync();
        }

        // Helper method to check if a room is available during a specific time
        private async Task<bool> IsRoomAvailableAsync(int roomId, DateTime startTime, DateTime endTime, int? excludeSessionId)
        {
            // Get all sessions in this room that overlap with the specified time range
            var overlappingSessions = await _unitOfWork.Sessions.FindAsync(s => 
                s.RoomId == roomId && 
                s.StartTime < endTime && 
                s.EndTime > startTime &&
                (excludeSessionId == null || s.Id != excludeSessionId.Value));

            return !overlappingSessions.Any();
        }
    }
}