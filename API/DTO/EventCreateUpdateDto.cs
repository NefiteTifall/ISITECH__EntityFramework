using System.ComponentModel.DataAnnotations;
using ISITECH__EventsArea.Domain.Entities;

namespace ISITECH__EventsArea.API.DTO
{
	public class EventCreateUpdateDto
	{
		public int? Id { get; set; }

		[Required] [StringLength(200)] public string Title { get; set; }

		[Required] public string Description { get; set; }

		[Required] public DateTime StartDate { get; set; }

		[Required] public DateTime EndDate { get; set; }

		[Required] public EventStatus Status { get; set; }

		[Required] public int CategoryId { get; set; }

		[Required] public int LocationId { get; set; }
	}
}