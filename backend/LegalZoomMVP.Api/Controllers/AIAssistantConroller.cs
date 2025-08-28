using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LegalZoomMVP.Application.DTOs;
using LegalZoomMVP.Application.Interfaces;
using LegalZoomMVP.Application.Exceptions;
using System.Security.Claims;

namespace LegalZoomMVP.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AIAssistantController : ControllerBase
    {
        private readonly IAIAssistantService _aiAssistantService;

        public AIAssistantController(IAIAssistantService aiAssistantService)
        {
            _aiAssistantService = aiAssistantService;
        }

        [HttpPost("conversations")]
        public async Task<ActionResult<AIConversationDto>> CreateConversation(CreateAIConversationDto request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var conversation = await _aiAssistantService.CreateConversationAsync(userId, request);
            return CreatedAtAction(nameof(GetConversation), new { id = conversation.Id }, conversation);
        }

        [HttpGet("conversations")]
        public async Task<ActionResult<IEnumerable<AIConversationDto>>> GetConversations()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var conversations = await _aiAssistantService.GetUserConversationsAsync(userId);
            return Ok(conversations);
        }

        [HttpGet("conversations/{id}")]
        public async Task<ActionResult<AIConversationDto>> GetConversation(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var conversation = await _aiAssistantService.GetConversationAsync(userId, id);
            
            if (conversation == null)
                return NotFound(new { message = "Conversation not found" });

            return Ok(conversation);
        }

        [HttpPost("conversations/{id}/messages")]
        public async Task<ActionResult<AIMessageDto>> SendMessage(int id, SendAIMessageDto request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            
            try
            {
                var response = await _aiAssistantService.SendMessageAsync(userId, id, request);
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("conversations/{id}")]
        public async Task<IActionResult> DeleteConversation(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var success = await _aiAssistantService.DeleteConversationAsync(userId, id);
            
            if (!success)
                return NotFound(new { message = "Conversation not found" });

            return NoContent();
        }
    }
}