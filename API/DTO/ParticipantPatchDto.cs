using System.ComponentModel.DataAnnotations;

namespace ISITECH__EventsArea.API.DTO
{
    public class ParticipantPatchDto
    {
        public int? Id { get; set; }
        
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Le prénom doit contenir entre 2 et 100 caractères")]
        public string? FirstName { get; set; }
        
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Le nom doit contenir entre 2 et 100 caractères")]
        public string? LastName { get; set; }
        
        [EmailAddress(ErrorMessage = "L'email n'est pas valide")]
        [StringLength(100, ErrorMessage = "L'email ne peut pas dépasser 100 caractères")]
        public string? Email { get; set; }
        
        [StringLength(100, ErrorMessage = "Le nom de l'entreprise ne peut pas dépasser 100 caractères")]
        public string? Company { get; set; }
        
        [StringLength(100, ErrorMessage = "Le nom du poste ne peut pas dépasser 100 caractères")]
        public string? JobTitle { get; set; }
    }
}