using System.Collections.Generic;

namespace ISITECH__EventsArea.Domain.Entities
{
	public class Speaker
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Bio { get; set; }
		public string Email { get; set; }
		public string Company { get; set; }
        
		// Navigation properties
		public ICollection<SessionSpeaker> SessionSpeakers { get; set; } = new List<SessionSpeaker>();
	}
}