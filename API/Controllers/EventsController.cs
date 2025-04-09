using ISITECH__EventsArea.Domain.Entities;
using ISITECH__EventsArea.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ISITECH__EventsArea.Domain.Services;

namespace ISITECH__EventsArea.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        // GET: api/Events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] int? categoryId,
            [FromQuery] int? locationId,
            [FromQuery] EventStatus? status,
            [FromQuery] string searchTerm,
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _eventService.GetFilteredEventsAsync(
                startDate, endDate, categoryId, locationId, status,
                searchTerm, pageIndex, pageSize);

            // Ajout de l'en-tête pour la pagination
            Response.Headers.Add("X-Total-Count", result.TotalCount.ToString());

            return Ok(result.Events);
        }

        // GET: api/Events/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            var eventEntity = await _eventService.GetEventByIdAsync(id);

            if (eventEntity == null)
            {
                return NotFound();
            }

            return Ok(eventEntity);
        }

        // POST: api/Events
        [HttpPost]
        public async Task<ActionResult<Event>> CreateEvent(Event eventEntity)
        {
            await _eventService.CreateEventAsync(eventEntity);

            return CreatedAtAction(nameof(GetEvent), new { id = eventEntity.Id }, eventEntity);
        }

        // PUT: api/Events/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, Event eventEntity)
        {
            if (id != eventEntity.Id)
            {
                return BadRequest();
            }

            var exists = await _eventService.EventExistsAsync(id);
            
            if (!exists)
            {
                return NotFound();
            }

            await _eventService.UpdateEventAsync(eventEntity);

            return NoContent();
        }

        // DELETE: api/Events/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            await _eventService.DeleteEventAsync(id);

            return NoContent();
        }
    }
}