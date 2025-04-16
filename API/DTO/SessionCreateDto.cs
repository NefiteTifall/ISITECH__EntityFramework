using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ISITECH__EventsArea.API.DTO
{
    public class SessionCreateDto
    {
        public int? Id { get; set; }
        
        [Required(ErrorMessage = "Le titre est obligatoire")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Le titre doit contenir entre 3 et 200 caractères")]
        public string Title { get; set; }
        
        [Required(ErrorMessage = "La description est obligatoire")]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "La description doit contenir entre 10 et 2000 caractères")]
        public string Description { get; set; }
        
        [Required(ErrorMessage = "L'heure de début est obligatoire")]
        [DataType(DataType.DateTime)]
        public DateTime StartTime { get; set; }
        
        [Required(ErrorMessage = "L'heure de fin est obligatoire")]
        [DataType(DataType.DateTime)]
        public DateTime EndTime { get; set; }
        
        [Required(ErrorMessage = "L'événement est obligatoire")]
        [Range(1, int.MaxValue, ErrorMessage = "Veuillez sélectionner un événement valide")]
        public int EventId { get; set; }
        
        // RoomId is optional when creating a session
        public int? RoomId { get; set; }
        
        // Optional list of speaker IDs to associate with the session
        public List<SessionSpeakerCreateDto> Speakers { get; set; }
    }
    
    public class SessionSpeakerCreateDto
    {
        [Required(ErrorMessage = "L'identifiant de l'intervenant est obligatoire")]
        [Range(1, int.MaxValue, ErrorMessage = "Veuillez sélectionner un intervenant valide")]
        public int SpeakerId { get; set; }
        
        [Required(ErrorMessage = "Le rôle de l'intervenant est obligatoire")]
        [StringLength(100, ErrorMessage = "Le rôle ne peut pas dépasser 100 caractères")]
        public string Role { get; set; }
    }
}