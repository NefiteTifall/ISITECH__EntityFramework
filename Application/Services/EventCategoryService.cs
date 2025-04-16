using AutoMapper;
using ISITECH__EventsArea.API.DTO;
using ISITECH__EventsArea.Domain.Entities;
using ISITECH__EventsArea.Domain.Interfaces;
using ISITECH__EventsArea.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISITECH__EventsArea.Application.Services
{
    public class EventCategoryService : IEventCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EventCategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EventCategory> GetCategoryEntityByIdAsync(int id)
        {
            return await _unitOfWork.EventCategories.GetCategoryWithEventsAsync(id);
        }

        public async Task<EventCategoryDto> GetCategoryByIdAsync(int id)
        {
            var category = await _unitOfWork.EventCategories.GetCategoryWithEventsAsync(id);
            if (category == null)
                return null;

            var categoryDto = _mapper.Map<EventCategoryDto>(category);
            categoryDto.EventCount = category.Events?.Count ?? 0;
            return categoryDto;
        }

        public async Task<IEnumerable<EventCategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.EventCategories.GetAllCategoriesWithEventCountAsync();
            return categories.Select(c => {
                var dto = _mapper.Map<EventCategoryDto>(c);
                dto.EventCount = c.Events?.Count ?? 0;
                return dto;
            });
        }

        public async Task<(IEnumerable<EventCategoryDto> Categories, int TotalCount)> GetPagedCategoriesAsync(
            string searchTerm,
            int pageIndex,
            int pageSize)
        {
            var result = await _unitOfWork.EventCategories.GetPagedCategoriesWithEventCountAsync(
                searchTerm, pageIndex, pageSize);
                
            var dtos = result.Categories.Select(c => {
                var dto = _mapper.Map<EventCategoryDto>(c);
                dto.EventCount = c.Events?.Count ?? 0;
                return dto;
            });
            
            return (dtos, result.TotalCount);
        }

        public async Task<EventCategoryDto> CreateCategoryAsync(EventCategoryCreateDto categoryDto)
        {
            var category = _mapper.Map<EventCategory>(categoryDto);
            await _unitOfWork.EventCategories.AddAsync(category);
            await _unitOfWork.CompleteAsync();
            
            return _mapper.Map<EventCategoryDto>(category);
        }

        public async Task UpdateCategoryAsync(int id, EventCategoryPatchDto categoryDto)
        {
            var category = await _unitOfWork.EventCategories.GetByIdAsync(id);
            if (category == null)
            {
                throw new KeyNotFoundException($"Catégorie avec l'ID {id} non trouvée.");
            }

            // Apply changes only if not null
            if (categoryDto.Name != null)
                category.Name = categoryDto.Name;
                
            if (categoryDto.Description != null)
                category.Description = categoryDto.Description;

            _unitOfWork.EventCategories.Update(category);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _unitOfWork.EventCategories.GetByIdAsync(id);
            if (category == null)
            {
                throw new KeyNotFoundException($"Catégorie avec l'ID {id} non trouvée.");
            }
            
            // Check if category has events
            var categoryWithEvents = await _unitOfWork.EventCategories.GetCategoryWithEventsAsync(id);
            if (categoryWithEvents.Events != null && categoryWithEvents.Events.Any())
            {
                throw new InvalidOperationException("Impossible de supprimer une catégorie qui contient des événements.");
            }
            
            _unitOfWork.EventCategories.Remove(category);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<bool> CategoryExistsAsync(int id)
        {
            return await _unitOfWork.EventCategories.ExistsAsync(c => c.Id == id);
        }
    }
}