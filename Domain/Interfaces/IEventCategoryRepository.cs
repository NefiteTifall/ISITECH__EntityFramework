using System.Collections.Generic;
using System.Threading.Tasks;
using ISITECH__EventsArea.Domain.Entities;

namespace ISITECH__EventsArea.Domain.Interfaces
{
    public interface IEventCategoryRepository : IRepository<EventCategory>
    {
        Task<EventCategory> GetCategoryWithEventsAsync(int id);
        Task<IEnumerable<EventCategory>> GetAllCategoriesWithEventCountAsync();
        Task<(IEnumerable<EventCategory> Categories, int TotalCount)> GetPagedCategoriesWithEventCountAsync(
            string searchTerm,
            int pageIndex,
            int pageSize);
    }
}