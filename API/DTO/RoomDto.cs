using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ISITECH__EventsArea.API.DTO
{
    public class RoomDto
    {
        [Required(ErrorMessage = "L'identifiant est obligatoire")]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Le nom est obligatoire")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Le nom doit contenir entre 2 et 100 caractères")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "La capacité est obligatoire")]
        [Range(1, int.MaxValue, ErrorMessage = "La capacité doit être un nombre positif")]
        public int Capacity { get; set; }
        
        [Required(ErrorMessage = "L'identifiant du lieu est obligatoire")]
        public int LocationId { get; set; }
        
        [Required(ErrorMessage = "Le nom du lieu est obligatoire")]
        public string LocationName { get; set; }
        
        // We don't include navigation properties directly in the DTO
        // but we could include counts or summary information
        public int SessionCount { get; set; }
        
        // We might want to include a list of sessions for display purposes
        public ICollection<SessionSummaryForRoomDto> Sessions { get; set; }
    }
    
    // A simplified DTO for displaying session information in room lists
    public class SessionSummaryForRoomDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string EventTitle { get; set; }
        public System.DateTime StartTime { get; set; }
        public System.DateTime EndTime { get; set; }
    }
}