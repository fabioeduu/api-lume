using Lume.Api.DTOs;
using Lume.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lume.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly ILogger<ChatController> _logger;

        public ChatController(IChatService chatService, ILogger<ChatController> logger)
        {
            _chatService = chatService;
            _logger = logger;
        }

        private int GetUserIdFromClaims()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : 0;
        }

        [HttpPost("message")]
        [ProducesResponseType(typeof(ChatMessageDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ChatMessageDto>> SendMessage([FromBody] CreateChatMessageDto messageDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetUserIdFromClaims();
            if (userId == 0)
                return Unauthorized();

            var result = await _chatService.SendMessageAsync(userId, messageDto);
            if (result == null)
                return BadRequest(new { message = "Failed to send message" });

            return CreatedAtAction(nameof(SendMessage), result);
        }

        [HttpGet("history/{userId}")]
        [ProducesResponseType(typeof(IEnumerable<ChatMessageDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ChatMessageDto>>> GetChatHistory(
            int userId,
            [FromQuery] int? limit = null)
        {
            IEnumerable<ChatMessageDto> result;

            if (limit.HasValue && limit > 0)
            {
                result = await _chatService.GetChatHistoryAsync(userId, limit.Value);
            }
            else
            {
                result = await _chatService.GetChatHistoryAsync(userId);
            }

            return Ok(result);
        }

        [HttpGet("history")]
        [ProducesResponseType(typeof(IEnumerable<ChatMessageDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ChatMessageDto>>> GetMyHistory([FromQuery] int? limit = null)
        {
            var userId = GetUserIdFromClaims();
            if (userId == 0)
                return Unauthorized();

            IEnumerable<ChatMessageDto> result;

            if (limit.HasValue && limit > 0)
            {
                result = await _chatService.GetChatHistoryAsync(userId, limit.Value);
            }
            else
            {
                result = await _chatService.GetChatHistoryAsync(userId);
            }

            return Ok(result);
        }

        [HttpDelete("message/{messageId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteMessage(int messageId)
        {
            var result = await _chatService.DeleteChatMessageAsync(messageId);
            if (!result)
                return NotFound(new { message = "Message not found" });

            return Ok(new { message = "Message deleted successfully" });
        }
    }
}
