// API/Controllers/EventsController.cs

using ISITECH__EventsArea.API.DTO;
using ISITECH__EventsArea.Domain.Entities;
using ISITECH__EventsArea.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
		public async Task<ActionResult<IEnumerable<EventDto>>> GetEvents(
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
		public async Task<ActionResult<EventDto>> GetEvent(int id)
		{
			var eventDto = await _eventService.GetEventByIdAsync(id);

			if (eventDto == null)
			{
				return NotFound();
			}

			return Ok(eventDto);
		}

		// POST: api/Events
		[HttpPost]
		public async Task<ActionResult<EventDto>> CreateEvent(EventCreateUpdateDto eventDto)
		{
			try
			{
				var createdEvent = await _eventService.CreateEventAsync(eventDto);
				return CreatedAtAction(nameof(GetEvent), new { id = createdEvent.Id }, createdEvent);
			}
			catch (ArgumentException ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// PUT: api/Events/5
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateEvent(int id, EventCreateUpdateDto eventDto)
		{
			if (id != eventDto.Id)
			{
				return BadRequest("L'ID dans l'URL ne correspond pas à l'ID dans les données.");
			}

			var exists = await _eventService.EventExistsAsync(id);

			if (!exists)
			{
				return NotFound();
			}

			try
			{
				await _eventService.UpdateEventAsync(eventDto);
				return NoContent();
			}
			catch (ArgumentException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(ex.Message);
			}
		}

		// DELETE: api/Events/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteEvent(int id)
		{
			try
			{
				await _eventService.DeleteEventAsync(id);
				return NoContent();
			}
			catch (KeyNotFoundException)
			{
				return NotFound();
			}
		}
	}
}