using System.ComponentModel.DataAnnotations;

namespace ISITECH__EventsArea.API.DTO
{
    public class RatingCreateDto
    {
        public int? Id { get; set; }
        
        [Required(ErrorMessage = "L'identifiant de la session est obligatoire")]
        [Range(1, int.MaxValue, ErrorMessage = "Veuillez sélectionner une session valide")]
        public int SessionId { get; set; }
        
        [Required(ErrorMessage = "L'identifiant du participant est obligatoire")]
        [Range(1, int.MaxValue, ErrorMessage = "Veuillez sélectionner un participant valide")]
        public int ParticipantId { get; set; }
        
        [Required(ErrorMessage = "La note est obligatoire")]
        [Range(1, 5, ErrorMessage = "La note doit être comprise entre 1 et 5")]
        public int Score { get; set; }
        
        [StringLength(1000, ErrorMessage = "Le commentaire ne peut pas dépasser 1000 caractères")]
        public string Comment { get; set; }
        
        // CreatedAt will be set automatically by the service
    }
}