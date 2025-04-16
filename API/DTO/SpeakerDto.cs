using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ISITECH__EventsArea.API.DTO
{
    public class SpeakerDto
    {
        [Required(ErrorMessage = "L'identifiant est obligatoire")]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Le prénom est obligatoire")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Le prénom doit contenir entre 2 et 100 caractères")]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "Le nom est obligatoire")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Le nom doit contenir entre 2 et 100 caractères")]
        public string LastName { get; set; }
        
        // Full name property for convenience
        public string FullName => $"{FirstName} {LastName}";
        
        [Required(ErrorMessage = "La biographie est obligatoire")]
        [StringLength(2000, ErrorMessage = "La biographie ne peut pas dépasser 2000 caractères")]
        public string Bio { get; set; }
        
        [Required(ErrorMessage = "L'email est obligatoire")]
        [EmailAddress(ErrorMessage = "L'email n'est pas valide")]
        [StringLength(100, ErrorMessage = "L'email ne peut pas dépasser 100 caractères")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "L'entreprise est obligatoire")]
        [StringLength(100, ErrorMessage = "Le nom de l'entreprise ne peut pas dépasser 100 caractères")]
        public string Company { get; set; }
        
        // We don't include navigation properties directly in the DTO
        // but we could include counts or summary information
        public int SessionCount { get; set; }
        
        // We might want to include a list of sessions for display purposes
        public ICollection<SessionSummaryDto> Sessions { get; set; }
    }
    
    // A simplified DTO for displaying session information in speaker lists
    public class SessionSummaryDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string EventTitle { get; set; }
        public string Role { get; set; }
    }
}