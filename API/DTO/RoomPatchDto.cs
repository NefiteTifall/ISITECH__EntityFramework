using System.ComponentModel.DataAnnotations;

namespace ISITECH__EventsArea.API.DTO
{
    public class RoomPatchDto
    {
        public int? Id { get; set; }
        
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Le nom doit contenir entre 2 et 100 caractères")]
        public string? Name { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "La capacité doit être un nombre positif")]
        public int? Capacity { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "Veuillez sélectionner un lieu valide")]
        public int? LocationId { get; set; }
    }
}