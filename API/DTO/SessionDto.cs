using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ISITECH__EventsArea.API.DTO
{
    public class SessionDto
    {
        [Required(ErrorMessage = "L'identifiant est obligatoire")]
        public int Id { get; set; }
        
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
        public int EventId { get; set; }
        
        public string EventTitle { get; set; }
        
        public int? RoomId { get; set; }
        
        public string RoomName { get; set; }
        
        // We don't include navigation properties directly in the DTO
        // but we could include counts or summary information
        public int SpeakerCount { get; set; }
        public int RatingCount { get; set; }
        public double AverageRating { get; set; }
        
        // We might want to include a list of speakers for display purposes
        public ICollection<SpeakerSummaryDto> Speakers { get; set; }
    }
    
    // A simplified DTO for displaying speaker information in session lists
    public class SpeakerSummaryDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
    }
}