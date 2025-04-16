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
    public class RatingsController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public RatingsController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        // GET: api/Ratings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rating>>> GetRatings(
            [FromQuery] int? sessionId,
            [FromQuery] int? participantId,
            [FromQuery] int? minScore,
            [FromQuery] int? maxScore,
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _ratingService.GetFilteredRatingsAsync(
                sessionId, participantId, minScore, maxScore, pageIndex, pageSize);

            // Add pagination header
            Response.Headers.Add("X-Total-Count", result.TotalCount.ToString());

            return Ok(result.Ratings);
        }

        // GET: api/Ratings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Rating>> GetRating(int id)
        {
            try
            {
                var rating = await _ratingService.GetRatingByIdAsync(id);
                if (rating == null)
                {
                    return NotFound();
                }
                return Ok(rating);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // GET: api/Ratings/Session/5
        [HttpGet("Session/{sessionId}")]
        public async Task<ActionResult<IEnumerable<Rating>>> GetRatingsBySession(int sessionId)
        {
            try
            {
                var ratings = await _ratingService.GetRatingsBySessionAsync(sessionId);
                return Ok(ratings);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // GET: api/Ratings/Participant/5
        [HttpGet("Participant/{participantId}")]
        public async Task<ActionResult<IEnumerable<Rating>>> GetRatingsByParticipant(int participantId)
        {
            try
            {
                var ratings = await _ratingService.GetRatingsByParticipantAsync(participantId);
                return Ok(ratings);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // GET: api/Ratings/Session/5/Average
        [HttpGet("Session/{sessionId}/Average")]
        public async Task<ActionResult<double>> GetAverageRatingForSession(int sessionId)
        {
            try
            {
                var averageRating = await _ratingService.GetAverageRatingForSessionAsync(sessionId);
                return Ok(averageRating);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // GET: api/Ratings/Event/5/Averages
        [HttpGet("Event/{eventId}/Averages")]
        public async Task<ActionResult<Dictionary<int, double>>> GetAverageRatingsForEvent(int eventId)
        {
            try
            {
                var averageRatings = await _ratingService.GetAverageRatingsForEventAsync(eventId);
                return Ok(averageRatings);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // POST: api/Ratings
        [HttpPost]
        public async Task<ActionResult<Rating>> CreateRating(Rating rating)
        {
            try
            {
                var createdRating = await _ratingService.CreateRatingAsync(rating);
                return CreatedAtAction(nameof(GetRating), new { id = createdRating.Id }, createdRating);
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

        // PUT: api/Ratings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRating(int id, Rating rating)
        {
            if (id != rating.Id)
            {
                return BadRequest("ID mismatch between URL and body");
            }

            try
            {
                await _ratingService.UpdateRatingAsync(rating);
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
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Ratings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRating(int id)
        {
            try
            {
                await _ratingService.DeleteRatingAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
