using System;

namespace ISITECH__EventsArea.Domain.Entities
{
	public class EventParticipant
	{
		public int EventId { get; set; }
		public int ParticipantId { get; set; }
		public DateTime RegistrationDate { get; set; }
		public AttendanceStatus AttendanceStatus { get; set; }
        
		// Navigation properties
		public Event Event { get; set; }
		public Participant Participant { get; set; }
	}
    
	public enum AttendanceStatus
	{
		Registered,
		Confirmed,
		Attended,
		Cancelled,
		NoShow
	}
}