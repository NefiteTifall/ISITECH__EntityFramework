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
    public class SpeakerService : ISpeakerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SpeakerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Speaker> GetSpeakerEntityByIdAsync(int id)
        {
            return await _unitOfWork.Speakers.GetSpeakerWithDetailsAsync(id);
        }

        public async Task<Speaker> GetSpeakerByIdAsync(int id)
        {
            return await _unitOfWork.Speakers.GetSpeakerWithDetailsAsync(id);
        }

        public async Task<IEnumerable<Speaker>> GetAllSpeakersAsync()
        {
            return await _unitOfWork.Speakers.GetAllAsync();
        }

        public async Task<IEnumerable<Speaker>> GetSpeakersBySessionAsync(int sessionId)
        {
            var session = await _unitOfWork.Sessions.GetByIdAsync(sessionId);
            if (session == null)
            {
                throw new KeyNotFoundException($"Session with ID {sessionId} not found.");
            }

            return await _unitOfWork.Speakers.GetSpeakersBySessionAsync(sessionId);
        }

        public async Task<IEnumerable<Speaker>> GetSpeakersByCompanyAsync(string company)
        {
            if (string.IsNullOrWhiteSpace(company))
            {
                throw new ArgumentException("Company name is required.");
            }

            return await _unitOfWork.Speakers.GetSpeakersByCompanyAsync(company);
        }

        public async Task<(IEnumerable<Speaker> Speakers, int TotalCount)> GetFilteredSpeakersAsync(
            string searchTerm,
            string company,
            int pageIndex,
            int pageSize)
        {
            return await _unitOfWork.Speakers.GetFilteredSpeakersAsync(
                searchTerm,
                company,
                pageIndex,
                pageSize);
        }

        public async Task<Speaker> CreateSpeakerAsync(Speaker speaker)
        {
            // Validate speaker data
            if (string.IsNullOrWhiteSpace(speaker.FirstName))
            {
                throw new ArgumentException("Speaker first name is required.");
            }

            if (string.IsNullOrWhiteSpace(speaker.LastName))
            {
                throw new ArgumentException("Speaker last name is required.");
            }

            if (string.IsNullOrWhiteSpace(speaker.Email))
            {
                throw new ArgumentException("Speaker email is required.");
            }

            // Check if email is already in use
            var existingSpeaker = await _unitOfWork.Speakers.SingleOrDefaultAsync(s => s.Email == speaker.Email);
            if (existingSpeaker != null)
            {
                throw new ArgumentException($"A speaker with email {speaker.Email} already exists.");
            }

            await _unitOfWork.Speakers.AddAsync(speaker);
            await _unitOfWork.CompleteAsync();
            
            return speaker;
        }

        public async Task UpdateSpeakerAsync(Speaker speaker)
        {
            var existingSpeaker = await _unitOfWork.Speakers.GetByIdAsync(speaker.Id);
            if (existingSpeaker == null)
            {
                throw new KeyNotFoundException($"Speaker with ID {speaker.Id} not found.");
            }

            // Validate speaker data
            if (string.IsNullOrWhiteSpace(speaker.FirstName))
            {
                throw new ArgumentException("Speaker first name is required.");
            }

            if (string.IsNullOrWhiteSpace(speaker.LastName))
            {
                throw new ArgumentException("Speaker last name is required.");
            }

            if (string.IsNullOrWhiteSpace(speaker.Email))
            {
                throw new ArgumentException("Speaker email is required.");
            }

            // Check if email is already in use by another speaker
            if (speaker.Email != existingSpeaker.Email)
            {
                var emailExists = await _unitOfWork.Speakers.ExistsAsync(s => s.Email == speaker.Email && s.Id != speaker.Id);
                if (emailExists)
                {
                    throw new ArgumentException($"A speaker with email {speaker.Email} already exists.");
                }
            }

            _unitOfWork.Speakers.Update(speaker);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteSpeakerAsync(int id)
        {
            var speaker = await _unitOfWork.Speakers.GetByIdAsync(id);
            if (speaker == null)
            {
                throw new KeyNotFoundException($"Speaker with ID {id} not found.");
            }

            // Check if the speaker is assigned to any sessions
            var hasSessions = await _unitOfWork.Sessions.ExistsAsync(s => 
                s.SessionSpeakers.Any(ss => ss.SpeakerId == id));

            if (hasSessions)
            {
                throw new InvalidOperationException($"Cannot delete speaker with ID {id} because they are assigned to one or more sessions.");
            }

            _unitOfWork.Speakers.Remove(speaker);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<bool> SpeakerExistsAsync(int id)
        {
            return await _unitOfWork.Speakers.ExistsAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Session>> GetSpeakerSessionsAsync(int speakerId)
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
    }
}