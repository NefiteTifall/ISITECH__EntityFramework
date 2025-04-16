using System.Collections.Generic;

namespace ISITECH__EventsArea.API.DTO
{
    public class EventCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int EventCount { get; set; }
    }
}