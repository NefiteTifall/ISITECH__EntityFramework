using System;
using System.Threading.Tasks;

namespace ISITECH__EventsArea.Domain.Interfaces
{
	public interface IUnitOfWork : IDisposable
	{
		IEventRepository Events { get; }
		IEventCategoryRepository EventCategories { get; }
		IParticipantRepository Participants { get; }
		ISessionRepository Sessions { get; }
		ISpeakerRepository Speakers { get; }
		ILocationRepository Locations { get; }
		IRoomRepository Rooms { get; }
		IRatingRepository Ratings { get; }

		Task<int> CompleteAsync();
	}
}
