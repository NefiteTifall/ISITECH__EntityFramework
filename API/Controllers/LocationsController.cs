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
    public class LocationsController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationsController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        // GET: api/Locations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Location>>> GetLocations(
            [FromQuery] string searchTerm,
            [FromQuery] string city,
            [FromQuery] string country,
            [FromQuery] int? minCapacity,
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _locationService.GetFilteredLocationsAsync(
                searchTerm, city, country, minCapacity, pageIndex, pageSize);

            // Add pagination header
            Response.Headers.Add("X-Total-Count", result.TotalCount.ToString());

            return Ok(result.Locations);
        }

        // GET: api/Locations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Location>> GetLocation(int id)
        {
            try
            {
                var location = await _locationService.GetLocationByIdAsync(id);
                if (location == null)
                {
                    return NotFound();
                }
                return Ok(location);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // GET: api/Locations/Country/{country}
        [HttpGet("Country/{country}")]
        public async Task<ActionResult<IEnumerable<Location>>> GetLocationsByCountry(string country)
        {
            try
            {
                var locations = await _locationService.GetLocationsByCountryAsync(country);
                return Ok(locations);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Locations/City/{city}
        [HttpGet("City/{city}")]
        public async Task<ActionResult<IEnumerable<Location>>> GetLocationsByCity(string city)
        {
            try
            {
                var locations = await _locationService.GetLocationsByCityAsync(city);
                return Ok(locations);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Locations/5/Events
        [HttpGet("{id}/Events")]
        public async Task<ActionResult<IEnumerable<Event>>> GetLocationEvents(int id)
        {
            try
            {
                var events = await _locationService.GetLocationEventsAsync(id);
                return Ok(events);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // GET: api/Locations/5/Rooms
        [HttpGet("{id}/Rooms")]
        public async Task<ActionResult<IEnumerable<Room>>> GetLocationRooms(int id)
        {
            try
            {
                var rooms = await _locationService.GetLocationRoomsAsync(id);
                return Ok(rooms);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // POST: api/Locations
        [HttpPost]
        public async Task<ActionResult<Location>> CreateLocation(Location location)
        {
            try
            {
                var createdLocation = await _locationService.CreateLocationAsync(location);
                return CreatedAtAction(nameof(GetLocation), new { id = createdLocation.Id }, createdLocation);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Locations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLocation(int id, Location location)
        {
            if (id != location.Id)
            {
                return BadRequest("ID mismatch between URL and body");
            }

            try
            {
                await _locationService.UpdateLocationAsync(location);
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

        // DELETE: api/Locations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            try
            {
                await _locationService.DeleteLocationAsync(id);
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