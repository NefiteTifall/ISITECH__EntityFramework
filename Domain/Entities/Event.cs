using ISITECH__EventsArea.Domain.Entities;

namespace ISITECH__EventsArea.Domain.Entities
{
	public class Event
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public EventStatus Status { get; set; }
		public int CategoryId { get; set; }
		public int LocationId { get; set; }
        
		// Navigation properties
		public EventCategory Category { get; set; }
		public Location Location { get; set; }
		public ICollection<Session> Sessions { get; set; } = new List<Session>();
		public ICollection<EventParticipant> EventParticipants { get; set; } = new List<EventParticipant>();
	}

	public enum EventStatus
	{
		Draft,
		Published,
		Cancelled,
		Completed
	}
}