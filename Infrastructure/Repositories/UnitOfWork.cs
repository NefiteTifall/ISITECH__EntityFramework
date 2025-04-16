using System;
using System.Threading.Tasks;
using ISITECH__EventsArea.Domain.Interfaces;
using ISITECH__EventsArea.Infrastructure.Data;

namespace ISITECH__EventsArea.Infrastructure.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly EventsAreasDbContext _context;
		private bool _disposed = false;

 	private IEventRepository _eventRepository;
 	private IEventCategoryRepository _eventCategoryRepository;
 	private IParticipantRepository _participantRepository;
 	private ISessionRepository _sessionRepository;
 	private ISpeakerRepository _speakerRepository;
 	private ILocationRepository _locationRepository;
 	private IRoomRepository _roomRepository;
 	private IRatingRepository _ratingRepository;

		public UnitOfWork(EventsAreasDbContext context)
		{
			_context = context;
		}

 	public IEventRepository Events => _eventRepository ??= new EventRepository(_context);

 	public IEventCategoryRepository EventCategories => _eventCategoryRepository ??= new EventCategoryRepository(_context);

 	public IParticipantRepository Participants => _participantRepository ??= new ParticipantRepository(_context);

		public ISessionRepository Sessions => _sessionRepository ??= new SessionRepository(_context);

		public ISpeakerRepository Speakers => _speakerRepository ??= new SpeakerRepository(_context);

		public ILocationRepository Locations => _locationRepository ??= new LocationRepository(_context);

		public IRoomRepository Rooms => _roomRepository ??= new RoomRepository(_context);

		public IRatingRepository Ratings => _ratingRepository ??= new RatingRepository(_context);

		public async Task<int> CompleteAsync()
		{
			return await _context.SaveChangesAsync();
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed && disposing)
			{
				_context.Dispose();
			}
			_disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
