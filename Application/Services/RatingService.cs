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
    public class RatingService : IRatingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RatingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Rating> GetRatingEntityByIdAsync(int id)
        {
            return await _unitOfWork.Ratings.GetRatingWithDetailsAsync(id);
        }

        public async Task<Rating> GetRatingByIdAsync(int id)
        {
            return await _unitOfWork.Ratings.GetRatingWithDetailsAsync(id);
        }

        public async Task<IEnumerable<Rating>> GetAllRatingsAsync()
        {
            return await _unitOfWork.Ratings.GetAllAsync();
        }

        public async Task<IEnumerable<Rating>> GetRatingsBySessionAsync(int sessionId)
        {
            var session = await _unitOfWork.Sessions.GetByIdAsync(sessionId);
            if (session == null)
            {
                throw new KeyNotFoundException($"Session with ID {sessionId} not found.");
            }

            return await _unitOfWork.Ratings.GetRatingsBySessionAsync(sessionId);
        }

        public async Task<IEnumerable<Rating>> GetRatingsByParticipantAsync(int participantId)
        {
            var participant = await _unitOfWork.Participants.GetByIdAsync(participantId);
            if (participant == null)
            {
                throw new KeyNotFoundException($"Participant with ID {participantId} not found.");
            }

            return await _unitOfWork.Ratings.GetRatingsByParticipantAsync(participantId);
        }

        public async Task<double> GetAverageRatingForSessionAsync(int sessionId)
        {
            var session = await _unitOfWork.Sessions.GetByIdAsync(sessionId);
            if (session == null)
            {
                throw new KeyNotFoundException($"Session with ID {sessionId} not found.");
            }

            return await _unitOfWork.Ratings.GetAverageRatingForSessionAsync(sessionId);
        }

        public async Task<(IEnumerable<Rating> Ratings, int TotalCount)> GetFilteredRatingsAsync(
            int? sessionId,
            int? participantId,
            int? minScore,
            int? maxScore,
            int pageIndex,
            int pageSize)
        {
            return await _unitOfWork.Ratings.GetFilteredRatingsAsync(
                sessionId,
                participantId,
                minScore,
                maxScore,
                pageIndex,
                pageSize);
        }

        public async Task<Rating> CreateRatingAsync(Rating rating)
        {
            // Validate rating data
            if (rating.Score < 1 || rating.Score > 5)
            {
                throw new ArgumentException("Rating score must be between 1 and 5.");
            }

            // Validate that the session exists
            var session = await _unitOfWork.Sessions.GetByIdAsync(rating.SessionId);
            if (session == null)
            {
                throw new KeyNotFoundException($"Session with ID {rating.SessionId} not found.");
            }

            // Validate that the participant exists
            var participant = await _unitOfWork.Participants.GetByIdAsync(rating.ParticipantId);
            if (participant == null)
            {
                throw new KeyNotFoundException($"Participant with ID {rating.ParticipantId} not found.");
            }

            // Check if the participant has already rated this session
            var existingRating = await _unitOfWork.Ratings.SingleOrDefaultAsync(
                r => r.SessionId == rating.SessionId && r.ParticipantId == rating.ParticipantId);
                
            if (existingRating != null)
            {
                throw new InvalidOperationException($"Participant with ID {rating.ParticipantId} has already rated session with ID {rating.SessionId}.");
            }

            // Set creation timestamp
            rating.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.Ratings.AddAsync(rating);
            await _unitOfWork.CompleteAsync();
            
            return rating;
        }

        public async Task UpdateRatingAsync(Rating rating)
        {
            var existingRating = await _unitOfWork.Ratings.GetByIdAsync(rating.Id);
            if (existingRating == null)
            {
                throw new KeyNotFoundException($"Rating with ID {rating.Id} not found.");
            }

            // Validate rating data
            if (rating.Score < 1 || rating.Score > 5)
            {
                throw new ArgumentException("Rating score must be between 1 and 5.");
            }

            // Validate that the session exists
            var session = await _unitOfWork.Sessions.GetByIdAsync(rating.SessionId);
            if (session == null)
            {
                throw new KeyNotFoundException($"Session with ID {rating.SessionId} not found.");
            }

            // Validate that the participant exists
            var participant = await _unitOfWork.Participants.GetByIdAsync(rating.ParticipantId);
            if (participant == null)
            {
                throw new KeyNotFoundException($"Participant with ID {rating.ParticipantId} not found.");
            }

            // Check if changing session or participant would create a duplicate
            if (rating.SessionId != existingRating.SessionId || rating.ParticipantId != existingRating.ParticipantId)
            {
                var duplicateRating = await _unitOfWork.Ratings.SingleOrDefaultAsync(
                    r => r.SessionId == rating.SessionId && 
                         r.ParticipantId == rating.ParticipantId && 
                         r.Id != rating.Id);
                    
                if (duplicateRating != null)
                {
                    throw new InvalidOperationException($"Participant with ID {rating.ParticipantId} has already rated session with ID {rating.SessionId}.");
                }
            }

            // Preserve the original creation timestamp
            rating.CreatedAt = existingRating.CreatedAt;

            _unitOfWork.Ratings.Update(rating);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteRatingAsync(int id)
        {
            var rating = await _unitOfWork.Ratings.GetByIdAsync(id);
            if (rating == null)
            {
                throw new KeyNotFoundException($"Rating with ID {id} not found.");
            }

            _unitOfWork.Ratings.Remove(rating);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<bool> RatingExistsAsync(int id)
        {
            return await _unitOfWork.Ratings.ExistsAsync(r => r.Id == id);
        }

        public async Task<Dictionary<int, double>> GetAverageRatingsForEventAsync(int eventId)
        {
            var eventEntity = await _unitOfWork.Events.GetByIdAsync(eventId);
            if (eventEntity == null)
            {
                throw new KeyNotFoundException($"Event with ID {eventId} not found.");
            }

            // Get all sessions for this event
            var sessions = await _unitOfWork.Sessions.FindAsync(s => s.EventId == eventId);
            
            var result = new Dictionary<int, double>();
            
            foreach (var session in sessions)
            {
                var averageRating = await _unitOfWork.Ratings.GetAverageRatingForSessionAsync(session.Id);
                result.Add(session.Id, averageRating);
            }
            
            return result;
        }
    }
}