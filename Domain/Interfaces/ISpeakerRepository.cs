using System.Collections.Generic;
using System.Threading.Tasks;
using ISITECH__EventsArea.Domain.Entities;

namespace ISITECH__EventsArea.Domain.Interfaces
{
	public interface ISpeakerRepository : IRepository<Speaker>
	{
		Task<Speaker> GetSpeakerWithDetailsAsync(int id);
		Task<IEnumerable<Speaker>> GetSpeakersBySessionAsync(int sessionId);
		Task<IEnumerable<Speaker>> GetSpeakersByCompanyAsync(string company);
		Task<(IEnumerable<Speaker> Speakers, int TotalCount)> GetFilteredSpeakersAsync(
			string searchTerm,
			string company,
			int pageIndex,
			int pageSize);
	}
}