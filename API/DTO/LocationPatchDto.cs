using System.ComponentModel.DataAnnotations;

namespace ISITECH__EventsArea.API.DTO
{
    public class LocationPatchDto
    {
        public int? Id { get; set; }
        
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Le nom doit contenir entre 2 et 100 caractères")]
        public string? Name { get; set; }
        
        [StringLength(200, ErrorMessage = "L'adresse ne peut pas dépasser 200 caractères")]
        public string? Address { get; set; }
        
        [StringLength(100, ErrorMessage = "La ville ne peut pas dépasser 100 caractères")]
        public string? City { get; set; }
        
        [StringLength(100, ErrorMessage = "Le pays ne peut pas dépasser 100 caractères")]
        public string? Country { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "La capacité doit être un nombre positif")]
        public int? Capacity { get; set; }
    }
}