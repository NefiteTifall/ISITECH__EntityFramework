namespace ISITECH__EventsArea.API.DTO
{
	public class EventDto
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public string Status { get; set; }
		public string CategoryName { get; set; }
		public string LocationName { get; set; }
		public int CategoryId { get; set; }
		public int LocationId { get; set; }
	}
}