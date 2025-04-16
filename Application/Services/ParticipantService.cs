using AutoMapper;
using ISITECH__EventsArea.Domain.Entities;
using ISITECH__EventsArea.Domain.Interfaces;
using ISITECH__EventsArea.Domain.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISITECH__EventsArea.Application.Services
{
    public class ParticipantService : IParticipantService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ParticipantService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Participant> GetParticipantEntityByIdAsync(int id)
        {
            return await _unitOfWork.Participants.GetByIdAsync(id);
        }

        public async Task<Participant> GetParticipantByIdAsync(int id)
        {
            return await _unitOfWork.Participants.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Participant>> GetAllParticipantsAsync()
        {
            return await _unitOfWork.Participants.GetAllAsync();
        }

        public async Task<IEnumerable<Participant>> GetParticipantsByEventAsync(int eventId)
        {
            var eventEntity = await _unitOfWork.Events.GetEventWithDetailsAsync(eventId);
            if (eventEntity == null)
            {
                throw new KeyNotFoundException($"Event with ID {eventId} not found.");
            }

            var participants = new List<Participant>();
            foreach (var ep in eventEntity.EventParticipants)
            {
                participants.Add(ep.Participant);
            }

            return participants;
        }

        public async Task<(IEnumerable<Participant> Participants, int TotalCount)> GetFilteredParticipantsAsync(
            string searchTerm,
            string company,
            int pageIndex,
            int pageSize)
        {
            // Implement filtering logic using the repository
            var query = _unitOfWork.Participants.GetAllAsync().Result.AsQueryable();

            // Apply filters
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(p => 
                    p.FirstName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || 
                    p.LastName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || 
                    p.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    p.Company.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    p.JobTitle.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(company))
            {
                query = query.Where(p => p.Company.Equals(company, StringComparison.OrdinalIgnoreCase));
            }

            // Get total count
            int totalCount = query.Count();

            // Apply pagination
            var participants = query
                .OrderBy(p => p.LastName)
                .ThenBy(p => p.FirstName)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return (participants, totalCount);
        }

        public async Task<Participant> CreateParticipantAsync(Participant participant)
        {
            if (string.IsNullOrWhiteSpace(participant.Email))
            {
                throw new ArgumentException("Email is required.");
            }

            // Check if email is already in use
            var existingParticipant = await _unitOfWork.Participants.SingleOrDefaultAsync(p => p.Email == participant.Email);
            if (existingParticipant != null)
            {
                throw new ArgumentException($"A participant with email {participant.Email} already exists.");
            }

            await _unitOfWork.Participants.AddAsync(participant);
            await _unitOfWork.CompleteAsync();
            
            return participant;
        }

        public async Task UpdateParticipantAsync(Participant participant)
        {
            var existingParticipant = await _unitOfWork.Participants.GetByIdAsync(participant.Id);
            if (existingParticipant == null)
            {
                throw new KeyNotFoundException($"Participant with ID {participant.Id} not found.");
            }

            // Check if email is already in use by another participant
            if (!string.IsNullOrWhiteSpace(participant.Email) && participant.Email != existingParticipant.Email)
            {
                var emailExists = await _unitOfWork.Participants.ExistsAsync(p => p.Email == participant.Email && p.Id != participant.Id);
                if (emailExists)
                {
                    throw new ArgumentException($"A participant with email {participant.Email} already exists.");
                }
            }

            _unitOfWork.Participants.Update(participant);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteParticipantAsync(int id)
        {
            var participant = await _unitOfWork.Participants.GetByIdAsync(id);
            if (participant == null)
            {
                throw new KeyNotFoundException($"Participant with ID {id} not found.");
            }

            _unitOfWork.Participants.Remove(participant);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<bool> ParticipantExistsAsync(int id)
        {
            return await _unitOfWork.Participants.ExistsAsync(p => p.Id == id);
        }

        public async Task RegisterParticipantForEventAsync(int participantId, int eventId)
        {
            var participant = await _unitOfWork.Participants.GetByIdAsync(participantId);
            if (participant == null)
            {
                throw new KeyNotFoundException($"Participant with ID {participantId} not found.");
            }

            var eventEntity = await _unitOfWork.Events.GetByIdAsync(eventId);
            if (eventEntity == null)
            {
                throw new KeyNotFoundException($"Event with ID {eventId} not found.");
            }

            // Check if participant is already registered
            var isRegistered = await _unitOfWork.Events.ExistsAsync(e => 
                e.Id == eventId && 
                e.EventParticipants.Any(ep => ep.ParticipantId == participantId));

            if (isRegistered)
            {
                throw new InvalidOperationException($"Participant with ID {participantId} is already registered for event with ID {eventId}.");
            }

            var eventParticipant = new EventParticipant
            {
                EventId = eventId,
                ParticipantId = participantId,
                RegistrationDate = DateTime.UtcNow,
                AttendanceStatus = AttendanceStatus.Registered
            };

            // Add the registration
            eventEntity.EventParticipants.Add(eventParticipant);
            _unitOfWork.Events.Update(eventEntity);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UnregisterParticipantFromEventAsync(int participantId, int eventId)
        {
            var participant = await _unitOfWork.Participants.GetByIdAsync(participantId);
            if (participant == null)
            {
                throw new KeyNotFoundException($"Participant with ID {participantId} not found.");
            }

            var eventEntity = await _unitOfWork.Events.GetEventWithDetailsAsync(eventId);
            if (eventEntity == null)
            {
                throw new KeyNotFoundException($"Event with ID {eventId} not found.");
            }

            var registration = eventEntity.EventParticipants.FirstOrDefault(ep => ep.ParticipantId == participantId);
            if (registration == null)
            {
                throw new InvalidOperationException($"Participant with ID {participantId} is not registered for event with ID {eventId}.");
            }

            // Remove the registration
            eventEntity.EventParticipants.Remove(registration);
            _unitOfWork.Events.Update(eventEntity);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<Event>> GetParticipantEventHistoryAsync(int participantId)
        {
            var participant = await _unitOfWork.Participants.GetByIdAsync(participantId);
            if (participant == null)
            {
                throw new KeyNotFoundException($"Participant with ID {participantId} not found.");
            }

            // Get all events the participant has registered for
            var events = await _unitOfWork.Events.FindAsync(e => 
                e.EventParticipants.Any(ep => ep.ParticipantId == participantId));

            return events;
        }
    }
}