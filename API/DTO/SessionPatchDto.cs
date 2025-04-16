using System;
using System.ComponentModel.DataAnnotations;

namespace ISITECH__EventsArea.API.DTO
{
    public class SessionPatchDto
    {
        public int? Id { get; set; }
        
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Le titre doit contenir entre 3 et 200 caractères")]
        public string? Title { get; set; }
        
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "La description doit contenir entre 10 et 2000 caractères")]
        public string? Description { get; set; }
        
        [DataType(DataType.DateTime)]
        public DateTime? StartTime { get; set; }
        
        [DataType(DataType.DateTime)]
        public DateTime? EndTime { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "Veuillez sélectionner un événement valide")]
        public int? EventId { get; set; }
        
        public int? RoomId { get; set; }
    }
}