using Lume.Api.DTOs;
using Lume.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Lume.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        private int GetUserIdFromClaims()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : 0;
        }

        /// <summary>
        /// Get user profile
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>User profile data</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserProfileDto>> GetProfile(int id)
        {
            var result = await _userService.GetUserProfileAsync(id);
            if (result == null)
                return NotFound(new { message = "User not found" });

            return Ok(result);
        }

        /// <summary>
        /// Get current user profile
        /// </summary>
        /// <returns>Current user profile data</returns>
        [HttpGet("profile/me")]
        [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserProfileDto>> GetCurrentProfile()
        {
            var userId = GetUserIdFromClaims();
            if (userId == 0)
                return Unauthorized();

            var result = await _userService.GetUserProfileAsync(userId);
            if (result == null)
                return NotFound(new { message = "User not found" });

            return Ok(result);
        }

        /// <summary>
        /// Update user profile
        /// </summary>
        /// <param name="updateDto">Updated user data</param>
        /// <returns>Updated user profile</returns>
        [HttpPut]
        [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserProfileDto>> UpdateProfile([FromBody] UpdateUserDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetUserIdFromClaims();
            if (userId == 0)
                return Unauthorized();

            var result = await _userService.UpdateUserProfileAsync(userId, updateDto);
            if (result == null)
                return NotFound(new { message = "User not found" });

            return Ok(result);
        }

        /// <summary>
        /// Delete user account
        /// </summary>
        /// <returns>Success message</returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser()
        {
            var userId = GetUserIdFromClaims();
            if (userId == 0)
                return Unauthorized();

            var result = await _userService.DeleteUserAsync(userId);
            if (!result)
                return NotFound(new { message = "User not found" });

            return Ok(new { message = "User deleted successfully" });
        }
    }
}
