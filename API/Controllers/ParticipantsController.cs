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
    public class ParticipantsController : ControllerBase
    {
        private readonly IParticipantService _participantService;

        public ParticipantsController(IParticipantService participantService)
        {
            _participantService = participantService;
        }

        // GET: api/Participants
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Participant>>> GetParticipants(
            [FromQuery] string searchTerm,
            [FromQuery] string company,
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _participantService.GetFilteredParticipantsAsync(
                searchTerm, company, pageIndex, pageSize);

            // Add pagination header
            Response.Headers.Add("X-Total-Count", result.TotalCount.ToString());

            return Ok(result.Participants);
        }

        // GET: api/Participants/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Participant>> GetParticipant(int id)
        {
            try
            {
                var participant = await _participantService.GetParticipantByIdAsync(id);
                if (participant == null)
                {
                    return NotFound();
                }
                return Ok(participant);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // GET: api/Participants/5/Events
        [HttpGet("{id}/Events")]
        public async Task<ActionResult<IEnumerable<Event>>> GetParticipantEvents(int id)
        {
            try
            {
                var events = await _participantService.GetParticipantEventHistoryAsync(id);
                return Ok(events);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // POST: api/Participants
        [HttpPost]
        public async Task<ActionResult<Participant>> CreateParticipant(Participant participant)
        {
            try
            {
                var createdParticipant = await _participantService.CreateParticipantAsync(participant);
                return CreatedAtAction(nameof(GetParticipant), new { id = createdParticipant.Id }, createdParticipant);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Participants/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateParticipant(int id, Participant participant)
        {
            if (id != participant.Id)
            {
                return BadRequest("ID mismatch between URL and body");
            }

            try
            {
                await _participantService.UpdateParticipantAsync(participant);
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

        // DELETE: api/Participants/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParticipant(int id)
        {
            try
            {
                await _participantService.DeleteParticipantAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // POST: api/Participants/5/Register/2
        [HttpPost("{participantId}/Register/{eventId}")]
        public async Task<IActionResult> RegisterParticipantForEvent(int participantId, int eventId)
        {
            try
            {
                await _participantService.RegisterParticipantForEventAsync(participantId, eventId);
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

        // DELETE: api/Participants/5/Unregister/2
        [HttpDelete("{participantId}/Unregister/{eventId}")]
        public async Task<IActionResult> UnregisterParticipantFromEvent(int participantId, int eventId)
        {
            try
            {
                await _participantService.UnregisterParticipantFromEventAsync(participantId, eventId);
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