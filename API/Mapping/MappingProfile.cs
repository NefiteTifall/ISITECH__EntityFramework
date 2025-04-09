using AutoMapper;
using ISITECH__EventsArea.API.DTO;
using ISITECH__EventsArea.Domain.Entities;

namespace ISITECH__EventsArea.API.Mapping
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			// Event mappings
			CreateMap<Event, EventDto>()
				.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
				.ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
				.ForMember(dest => dest.LocationName, opt => opt.MapFrom(src => src.Location != null ? src.Location.Name : null));
            
			CreateMap<EventCreateUpdateDto, Event>();
		}
	}
}