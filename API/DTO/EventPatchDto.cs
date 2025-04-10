using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using ISITECH__EventsArea.Domain.Entities;

namespace ISITECH__EventsArea.API.DTO
{
	public class EventPatchDto
	{
		public int? Id { get; set; }

		[StringLength(200)] public string? Title { get; set; }

		public string? Description { get; set; }

		public DateTime? StartDate { get; set; }

		public DateTime? EndDate { get; set; }

		public EventStatus? Status { get; set; }

		public int? CategoryId { get; set; }

		public int? LocationId { get; set; }
	}
}