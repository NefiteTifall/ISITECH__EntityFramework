using System.ComponentModel.DataAnnotations;
using ISITECH__EventsArea.Domain.Entities;

namespace ISITECH__EventsArea.API.DTO
{
	public class EventCreateDto
	{
		public int? Id { get; set; }

		[Required(ErrorMessage = "Le titre est obligatoire")]
		[StringLength(200, MinimumLength = 3, ErrorMessage = "Le titre doit contenir entre 3 et 200 caractères")]
		public string Title { get; set; }

		[Required(ErrorMessage = "La description est obligatoire")]
		[StringLength(2000, MinimumLength = 10, ErrorMessage = "La description doit contenir entre 10 et 2000 caractères")]
		public string Description { get; set; }

		[Required(ErrorMessage = "La date de début est obligatoire")]
		[DataType(DataType.DateTime)]
		public DateTime StartDate { get; set; }

		[Required(ErrorMessage = "La date de fin est obligatoire")]
		[DataType(DataType.DateTime)]
		public DateTime EndDate { get; set; }

		[Required(ErrorMessage = "Le statut est obligatoire")]
		public EventStatus Status { get; set; }

		[Required(ErrorMessage = "La catégorie est obligatoire")]
		[Range(1, int.MaxValue, ErrorMessage = "Veuillez sélectionner une catégorie valide")]
		public int CategoryId { get; set; }

		[Required(ErrorMessage = "Le lieu est obligatoire")]
		[Range(1, int.MaxValue, ErrorMessage = "Veuillez sélectionner un lieu valide")]
		public int LocationId { get; set; }
	}

}