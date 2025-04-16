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
    public class SpeakersController : ControllerBase
    {
        private readonly ISpeakerService _speakerService;

        public SpeakersController(ISpeakerService speakerService)
        {
            _speakerService = speakerService;
        }

        // GET: api/Speakers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Speaker>>> GetSpeakers(
            [FromQuery] string searchTerm,
            [FromQuery] string company,
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _speakerService.GetFilteredSpeakersAsync(
                searchTerm, company, pageIndex, pageSize);

            // Add pagination header
            Response.Headers.Add("X-Total-Count", result.TotalCount.ToString());

            return Ok(result.Speakers);
        }

        // GET: api/Speakers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Speaker>> GetSpeaker(int id)
        {
            try
            {
                var speaker = await _speakerService.GetSpeakerByIdAsync(id);
                if (speaker == null)
                {
                    return NotFound();
                }
                return Ok(speaker);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // GET: api/Speakers/Session/5
        [HttpGet("Session/{sessionId}")]
        public async Task<ActionResult<IEnumerable<Speaker>>> GetSpeakersBySession(int sessionId)
        {
            try
            {
                var speakers = await _speakerService.GetSpeakersBySessionAsync(sessionId);
                return Ok(speakers);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // GET: api/Speakers/Company/{company}
        [HttpGet("Company/{company}")]
        public async Task<ActionResult<IEnumerable<Speaker>>> GetSpeakersByCompany(string company)
        {
            try
            {
                var speakers = await _speakerService.GetSpeakersByCompanyAsync(company);
                return Ok(speakers);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Speakers/5/Sessions
        [HttpGet("{id}/Sessions")]
        public async Task<ActionResult<IEnumerable<Session>>> GetSpeakerSessions(int id)
        {
            try
            {
                var sessions = await _speakerService.GetSpeakerSessionsAsync(id);
                return Ok(sessions);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // POST: api/Speakers
        [HttpPost]
        public async Task<ActionResult<Speaker>> CreateSpeaker(Speaker speaker)
        {
            try
            {
                var createdSpeaker = await _speakerService.CreateSpeakerAsync(speaker);
                return CreatedAtAction(nameof(GetSpeaker), new { id = createdSpeaker.Id }, createdSpeaker);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Speakers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSpeaker(int id, Speaker speaker)
        {
            if (id != speaker.Id)
            {
                return BadRequest("ID mismatch between URL and body");
            }

            try
            {
                await _speakerService.UpdateSpeakerAsync(speaker);
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

        // DELETE: api/Speakers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSpeaker(int id)
        {
            try
            {
                await _speakerService.DeleteSpeakerAsync(id);
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