// API/Mapping/MappingProfile.cs

using AutoMapper;
using ISITECH__EventsArea.API.DTO;
using ISITECH__EventsArea.Domain.Entities;

namespace ISITECH__EventsArea.API.Mapping
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			// Mapping pour Event
			CreateMap<Event, EventDto>()
				.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
				.ForMember(dest => dest.CategoryName,
					opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
				.ForMember(dest => dest.LocationName,
					opt => opt.MapFrom(src => src.Location != null ? src.Location.Name : null));

			// Mapping pour la création/mise à jour
			CreateMap<EventCreateDto, Event>()
				.ForMember(dest => dest.Id, opt => opt.Condition(src => src.Id.HasValue))
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value));

			// Vous ajouterez d'autres mappings ici au fur et à mesure
		}
	}
}