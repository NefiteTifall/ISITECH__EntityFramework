using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ISITECH__EventsArea.Domain.Entities;

namespace ISITECH__EventsArea.Domain.Interfaces
{
	public interface ISessionRepository : IRepository<Session>
	{
		Task<IEnumerable<Session>> GetSessionsByEventAsync(int eventId);
		Task<IEnumerable<Session>> GetSessionsByRoomAsync(int roomId);
		Task<IEnumerable<Session>> GetSessionsBySpeakerAsync(int speakerId);
		Task<IEnumerable<Session>> GetSessionsByTimeSlotAsync(int eventId, DateTime startTime, DateTime endTime);
		Task<Session> GetSessionWithDetailsAsync(int id);
		Task<bool> IsRoomAvailableForSessionAsync(int roomId, DateTime startTime, DateTime endTime, int? excludeSessionId = null);
		Task<double> GetSessionAverageRatingAsync(int sessionId);
		Task AddSessionSpeakerAsync(SessionSpeaker sessionSpeaker);
		void RemoveSessionSpeaker(SessionSpeaker sessionSpeaker);
		Task<SessionSpeaker> GetSessionSpeakerAsync(int sessionId, int speakerId);
	}
}