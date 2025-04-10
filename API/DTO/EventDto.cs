using System.ComponentModel.DataAnnotations;

namespace ISITECH__EventsArea.API.DTO
{
	public class EventDto
	{
		[Required(ErrorMessage = "L'identifiant est obligatoire")]
		public int Id { get; set; }
		
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
		public string Status { get; set; }
		
		[Required(ErrorMessage = "Le nom de la catégorie est obligatoire")]
		public string CategoryName { get; set; }
		
		[Required(ErrorMessage = "Le nom du lieu est obligatoire")]
		public string LocationName { get; set; }
		
		[Required(ErrorMessage = "L'identifiant de la catégorie est obligatoire")]
		[Range(1, int.MaxValue, ErrorMessage = "L'identifiant de la catégorie doit être un nombre positif")]
		public int CategoryId { get; set; }
		
		[Required(ErrorMessage = "L'identifiant du lieu est obligatoire")]
		[Range(1, int.MaxValue, ErrorMessage = "L'identifiant du lieu doit être un nombre positif")]
		public int LocationId { get; set; }
	}

}