using System;

namespace ISITECH__EventsArea.Domain.Entities
{
	public class Rating
	{
		public int Id { get; set; }
		public int SessionId { get; set; }
		public int ParticipantId { get; set; }
		public int Score { get; set; } // 1-5
		public string Comment { get; set; }
		public DateTime CreatedAt { get; set; }
        
		// Navigation properties
		public Session Session { get; set; }
		public Participant Participant { get; set; }
	}
}