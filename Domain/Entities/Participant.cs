using System.Collections.Generic;

namespace ISITECH__EventsArea.Domain.Entities
{
	public class Participant
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string Company { get; set; }
		public string JobTitle { get; set; }
        
		// Navigation properties
		public ICollection<EventParticipant> EventParticipants { get; set; } = new List<EventParticipant>();
		public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
	}
}