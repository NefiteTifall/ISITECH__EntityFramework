using System;
using System.Threading.Tasks;

namespace ISITECH__EventsArea.Domain.Interfaces
{
	public interface IUnitOfWork : IDisposable
	{
		IEventRepository Events { get; }
		IParticipantRepository Participants { get; }
		ISessionRepository Sessions { get; }
        
		Task<int> CompleteAsync();
	}
}