using Lume.Api.DTOs;
using Lume.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lume.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CheckinsController : ControllerBase
    {
        private readonly ICheckinService _checkinService;
        private readonly ILogger<CheckinsController> _logger;

        public CheckinsController(ICheckinService checkinService, ILogger<CheckinsController> logger)
        {
            _checkinService = checkinService;
            _logger = logger;
        }

        private int GetUserIdFromClaims()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : 0;
        }

        [HttpPost]
        [ProducesResponseType(typeof(CheckinDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CheckinDto>> CreateCheckin([FromBody] CreateCheckinDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetUserIdFromClaims();
            if (userId == 0)
                return Unauthorized();

            var result = await _checkinService.CreateCheckinAsync(userId, createDto);
            if (result == null)
                return BadRequest(new { message = "Invalid emotional level. Must be between 1 and 10" });

            return CreatedAtAction(nameof(GetCheckin), new { id = result.Id }, result);
        }

        /// <summary>
        /// Get a specific check-in
        /// </summary>
        /// <param name="id">Check-in ID</param>
        /// <returns>Check-in data</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CheckinDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CheckinDto>> GetCheckin(int id)
        {
            var result = await _checkinService.GetCheckinAsync(id);
            if (result == null)
                return NotFound(new { message = "Check-in not found" });

            return Ok(result);
        }

        [HttpGet("user/{userId}")]
        [ProducesResponseType(typeof(IEnumerable<CheckinHistoryDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CheckinHistoryDto>>> GetUserCheckins(
            int userId,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            IEnumerable<CheckinHistoryDto> result;

            if (fromDate.HasValue && toDate.HasValue)
            {
                result = await _checkinService.GetUserCheckinsAsync(userId, fromDate.Value, toDate.Value);
            }
            else
            {
                result = await _checkinService.GetUserCheckinsAsync(userId);
            }

            return Ok(result);
        }

        [HttpGet("my-checkins")]
        [ProducesResponseType(typeof(IEnumerable<CheckinHistoryDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CheckinHistoryDto>>> GetMyCheckins()
        {
            var userId = GetUserIdFromClaims();
            if (userId == 0)
                return Unauthorized();

            var result = await _checkinService.GetUserCheckinsAsync(userId);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(CheckinDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CheckinDto>> UpdateCheckin(int id, [FromBody] CreateCheckinDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _checkinService.UpdateCheckinAsync(id, updateDto);
            if (result == null)
                return NotFound(new { message = "Check-in not found or invalid emotional level" });

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCheckin(int id)
        {
            var result = await _checkinService.DeleteCheckinAsync(id);
            if (!result)
                return NotFound(new { message = "Check-in not found" });

            return Ok(new { message = "Check-in deleted successfully" });
        }
    }
}
