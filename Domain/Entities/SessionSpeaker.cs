namespace ISITECH__EventsArea.Domain.Entities
{
	public class SessionSpeaker
	{
		public int SessionId { get; set; }
		public int SpeakerId { get; set; }
		public string Role { get; set; }
        
		// Navigation properties
		public Session Session { get; set; }
		public Speaker Speaker { get; set; }
	}
}