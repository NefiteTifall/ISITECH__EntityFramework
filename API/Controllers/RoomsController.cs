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
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomsController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        // GET: api/Rooms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Room>>> GetRooms(
            [FromQuery] string searchTerm,
            [FromQuery] int? locationId,
            [FromQuery] int? minCapacity,
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _roomService.GetFilteredRoomsAsync(
                searchTerm, locationId, minCapacity, pageIndex, pageSize);

            // Add pagination header
            Response.Headers.Add("X-Total-Count", result.TotalCount.ToString());

            return Ok(result.Rooms);
        }

        // GET: api/Rooms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> GetRoom(int id)
        {
            try
            {
                var room = await _roomService.GetRoomByIdAsync(id);
                if (room == null)
                {
                    return NotFound();
                }
                return Ok(room);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // GET: api/Rooms/Location/5
        [HttpGet("Location/{locationId}")]
        public async Task<ActionResult<IEnumerable<Room>>> GetRoomsByLocation(int locationId)
        {
            try
            {
                var rooms = await _roomService.GetRoomsByLocationAsync(locationId);
                return Ok(rooms);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // GET: api/Rooms/Capacity/50
        [HttpGet("Capacity/{minCapacity}")]
        public async Task<ActionResult<IEnumerable<Room>>> GetRoomsByCapacity(int minCapacity)
        {
            try
            {
                var rooms = await _roomService.GetRoomsByCapacityAsync(minCapacity);
                return Ok(rooms);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Rooms/5/Sessions
        [HttpGet("{id}/Sessions")]
        public async Task<ActionResult<IEnumerable<Session>>> GetRoomSessions(int id)
        {
            try
            {
                var sessions = await _roomService.GetRoomSessionsAsync(id);
                return Ok(sessions);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // GET: api/Rooms/5/Available
        [HttpGet("{id}/Available")]
        public async Task<ActionResult<bool>> IsRoomAvailable(
            int id, 
            [FromQuery] DateTime startTime, 
            [FromQuery] DateTime endTime)
        {
            try
            {
                var isAvailable = await _roomService.IsRoomAvailableAsync(id, startTime, endTime);
                return Ok(isAvailable);
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

        // POST: api/Rooms
        [HttpPost]
        public async Task<ActionResult<Room>> CreateRoom(Room room)
        {
            try
            {
                var createdRoom = await _roomService.CreateRoomAsync(room);
                return CreatedAtAction(nameof(GetRoom), new { id = createdRoom.Id }, createdRoom);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Rooms/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoom(int id, Room room)
        {
            if (id != room.Id)
            {
                return BadRequest("ID mismatch between URL and body");
            }

            try
            {
                await _roomService.UpdateRoomAsync(room);
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
        }

        // DELETE: api/Rooms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            try
            {
                await _roomService.DeleteRoomAsync(id);
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