using System.Collections.Generic;

namespace ISITECH__EventsArea.Domain.Entities
{
	public class Room
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int Capacity { get; set; }
		public int LocationId { get; set; }
        
		// Navigation properties
		public Location Location { get; set; }
		public ICollection<Session> Sessions { get; set; } = new List<Session>();
	}
}