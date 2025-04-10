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
			[FromQuery] string? searchTerm,
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
		public async Task<ActionResult<EventDto>> CreateEvent(EventCreateDto eventDto)
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
		[HttpPatch("{id}")]
		public async Task<IActionResult> PatchEvent(int? id, [FromBody] EventPatchDto patchDto)
		{
			if (patchDto == null)
			{
				return BadRequest("Les données de mise à jour sont requises");
			}

			int eventId;
			if (id.HasValue && id.Value > 0)
			{
				// Utiliser l'ID de la route
				eventId = id.Value;
			}
			else if (patchDto.Id.HasValue && patchDto.Id.Value > 0)
			{
				// Utiliser l'ID du body si l'ID de la route n'est pas fourni
				eventId = patchDto.Id.Value;
			}
			else
			{
				return BadRequest("Un ID valide doit être fourni dans l'URL ou dans le corps de la requête");
			}

			try
			{
				await _eventService.PatchEventAsync(eventId, patchDto);
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