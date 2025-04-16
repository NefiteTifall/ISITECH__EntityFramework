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
    public class SessionsController : ControllerBase
    {
        private readonly ISessionService _sessionService;

        public SessionsController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        // GET: api/Sessions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Session>>> GetSessions(
            [FromQuery] int? eventId,
            [FromQuery] int? roomId,
            [FromQuery] int? speakerId,
            [FromQuery] string searchTerm,
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _sessionService.GetFilteredSessionsAsync(
                eventId, roomId, speakerId, searchTerm, pageIndex, pageSize);

            // Add pagination header
            Response.Headers.Add("X-Total-Count", result.TotalCount.ToString());

            return Ok(result.Sessions);
        }

        // GET: api/Sessions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Session>> GetSession(int id)
        {
            try
            {
                var session = await _sessionService.GetSessionByIdAsync(id);
                if (session == null)
                {
                    return NotFound();
                }
                return Ok(session);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // GET: api/Sessions/Event/5
        [HttpGet("Event/{eventId}")]
        public async Task<ActionResult<IEnumerable<Session>>> GetSessionsByEvent(int eventId)
        {
            try
            {
                var sessions = await _sessionService.GetSessionsByEventAsync(eventId);
                return Ok(sessions);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // GET: api/Sessions/Room/5
        [HttpGet("Room/{roomId}")]
        public async Task<ActionResult<IEnumerable<Session>>> GetSessionsByRoom(int roomId)
        {
            try
            {
                var sessions = await _sessionService.GetSessionsByRoomAsync(roomId);
                return Ok(sessions);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // GET: api/Sessions/Speaker/5
        [HttpGet("Speaker/{speakerId}")]
        public async Task<ActionResult<IEnumerable<Session>>> GetSessionsBySpeaker(int speakerId)
        {
            try
            {
                var sessions = await _sessionService.GetSessionsBySpeakerAsync(speakerId);
                return Ok(sessions);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // POST: api/Sessions
        [HttpPost]
        public async Task<ActionResult<Session>> CreateSession(Session session)
        {
            try
            {
                var createdSession = await _sessionService.CreateSessionAsync(session);
                return CreatedAtAction(nameof(GetSession), new { id = createdSession.Id }, createdSession);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Sessions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSession(int id, Session session)
        {
            if (id != session.Id)
            {
                return BadRequest("ID mismatch between URL and body");
            }

            try
            {
                await _sessionService.UpdateSessionAsync(session);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Sessions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSession(int id)
        {
            try
            {
                await _sessionService.DeleteSessionAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // POST: api/Sessions/5/AssignRoom/2
        [HttpPost("{sessionId}/AssignRoom/{roomId}")]
        public async Task<IActionResult> AssignSessionToRoom(int sessionId, int roomId)
        {
            try
            {
                await _sessionService.AssignSessionToRoomAsync(sessionId, roomId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/Sessions/5/AddSpeaker/2
        [HttpPost("{sessionId}/AddSpeaker/{speakerId}")]
        public async Task<IActionResult> AddSpeakerToSession(int sessionId, int speakerId, [FromBody] string role)
        {
            try
            {
                await _sessionService.AddSpeakerToSessionAsync(sessionId, speakerId, role);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Sessions/5/RemoveSpeaker/2
        [HttpDelete("{sessionId}/RemoveSpeaker/{speakerId}")]
        public async Task<IActionResult> RemoveSpeakerFromSession(int sessionId, int speakerId)
        {
            try
            {
                await _sessionService.RemoveSpeakerFromSessionAsync(sessionId, speakerId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}