using ISITECH__EventsArea.API.DTO;
using ISITECH__EventsArea.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISITECH__EventsArea.Domain.Services
{
    public interface IEventCategoryService
    {
        Task<EventCategory> GetCategoryEntityByIdAsync(int id);
        Task<EventCategoryDto> GetCategoryByIdAsync(int id);
        Task<IEnumerable<EventCategoryDto>> GetAllCategoriesAsync();
        Task<(IEnumerable<EventCategoryDto> Categories, int TotalCount)> GetPagedCategoriesAsync(
            string searchTerm,
            int pageIndex,
            int pageSize);
        Task<EventCategoryDto> CreateCategoryAsync(EventCategoryCreateDto categoryDto);
        Task UpdateCategoryAsync(int id, EventCategoryPatchDto categoryDto);
        Task DeleteCategoryAsync(int id);
        Task<bool> CategoryExistsAsync(int id);
    }
}