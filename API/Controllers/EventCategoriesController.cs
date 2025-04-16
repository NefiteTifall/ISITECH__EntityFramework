using ISITECH__EventsArea.API.DTO;
using ISITECH__EventsArea.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISITECH__EventsArea.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventCategoriesController : ControllerBase
    {
        private readonly IEventCategoryService _categoryService;

        public EventCategoriesController(IEventCategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: api/EventCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventCategoryDto>>> GetCategories(
            [FromQuery] string searchTerm,
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _categoryService.GetPagedCategoriesAsync(
                searchTerm, pageIndex, pageSize);

            // Add pagination header
            Response.Headers.Add("X-Total-Count", result.TotalCount.ToString());

            return Ok(result.Categories);
        }

        // GET: api/EventCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EventCategoryDto>> GetCategory(int id)
        {
            var categoryDto = await _categoryService.GetCategoryByIdAsync(id);

            if (categoryDto == null)
            {
                return NotFound();
            }

            return Ok(categoryDto);
        }

        // POST: api/EventCategories
        [HttpPost]
        public async Task<ActionResult<EventCategoryDto>> CreateCategory(EventCategoryCreateDto categoryDto)
        {
            try
            {
                var createdCategory = await _categoryService.CreateCategoryAsync(categoryDto);
                return CreatedAtAction(nameof(GetCategory), new { id = createdCategory.Id }, createdCategory);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PATCH: api/EventCategories/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] EventCategoryPatchDto categoryDto)
        {
            if (categoryDto == null)
            {
                return BadRequest("Les données de mise à jour sont requises");
            }

            try
            {
                await _categoryService.UpdateCategoryAsync(id, categoryDto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/EventCategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                await _categoryService.DeleteCategoryAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}