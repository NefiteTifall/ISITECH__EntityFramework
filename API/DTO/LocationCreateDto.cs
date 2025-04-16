using System.ComponentModel.DataAnnotations;

namespace ISITECH__EventsArea.API.DTO
{
    public class LocationCreateDto
    {
        public int? Id { get; set; }
        
        [Required(ErrorMessage = "Le nom est obligatoire")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Le nom doit contenir entre 2 et 100 caractères")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "L'adresse est obligatoire")]
        [StringLength(200, ErrorMessage = "L'adresse ne peut pas dépasser 200 caractères")]
        public string Address { get; set; }
        
        [Required(ErrorMessage = "La ville est obligatoire")]
        [StringLength(100, ErrorMessage = "La ville ne peut pas dépasser 100 caractères")]
        public string City { get; set; }
        
        [Required(ErrorMessage = "Le pays est obligatoire")]
        [StringLength(100, ErrorMessage = "Le pays ne peut pas dépasser 100 caractères")]
        public string Country { get; set; }
        
        [Required(ErrorMessage = "La capacité est obligatoire")]
        [Range(1, int.MaxValue, ErrorMessage = "La capacité doit être un nombre positif")]
        public int Capacity { get; set; }
    }
}