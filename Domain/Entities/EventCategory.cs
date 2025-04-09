using System.Collections.Generic;
using ISITECH__EventsArea.Domain.Entities;

namespace ISITECH__EventsArea.Domain.Entities
{
	public class EventCategory
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
        
		// Navigation properties
		public ICollection<Event> Events { get; set; } = new List<Event>();
	}
}