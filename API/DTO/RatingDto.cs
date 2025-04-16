using System;
using System.ComponentModel.DataAnnotations;

namespace ISITECH__EventsArea.API.DTO
{
    public class RatingDto
    {
        [Required(ErrorMessage = "L'identifiant est obligatoire")]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "L'identifiant de la session est obligatoire")]
        public int SessionId { get; set; }
        
        [Required(ErrorMessage = "Le titre de la session est obligatoire")]
        public string SessionTitle { get; set; }
        
        [Required(ErrorMessage = "L'identifiant du participant est obligatoire")]
        public int ParticipantId { get; set; }
        
        [Required(ErrorMessage = "Le nom du participant est obligatoire")]
        public string ParticipantName { get; set; }
        
        [Required(ErrorMessage = "La note est obligatoire")]
        [Range(1, 5, ErrorMessage = "La note doit être comprise entre 1 et 5")]
        public int Score { get; set; }
        
        [StringLength(1000, ErrorMessage = "Le commentaire ne peut pas dépasser 1000 caractères")]
        public string Comment { get; set; }
        
        [Required(ErrorMessage = "La date de création est obligatoire")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }
        
        // Additional properties for display purposes
        public string EventTitle { get; set; }
    }
}