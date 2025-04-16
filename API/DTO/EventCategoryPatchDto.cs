using System.ComponentModel.DataAnnotations;

namespace ISITECH__EventsArea.API.DTO
{
    public class EventCategoryPatchDto
    {
        public int? Id { get; set; }
        
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }
        
        [StringLength(500)]
        public string Description { get; set; }
    }
}