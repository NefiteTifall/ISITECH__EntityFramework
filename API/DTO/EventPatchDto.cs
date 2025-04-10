using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using ISITECH__EventsArea.Domain.Entities;

namespace ISITECH__EventsArea.API.DTO
{
	public class EventPatchDto
	{
		public int? Id { get; set; }

		[StringLength(200, MinimumLength = 3, ErrorMessage = "Le titre doit contenir entre 3 et 200 caractères")]
		public string? Title { get; set; }

		[StringLength(2000, MinimumLength = 10, ErrorMessage = "La description doit contenir entre 10 et 2000 caractères")]
		public string? Description { get; set; }

		[DataType(DataType.DateTime)]
		public DateTime? StartDate { get; set; }

		[DataType(DataType.DateTime)]
		public DateTime? EndDate { get; set; }

		public EventStatus? Status { get; set; }

		[Range(1, int.MaxValue, ErrorMessage = "L'identifiant de la catégorie doit être un nombre positif")]
		public int? CategoryId { get; set; }

		[Range(1, int.MaxValue, ErrorMessage = "L'identifiant du lieu doit être un nombre positif")]
		public int? LocationId { get; set; }
	}

}