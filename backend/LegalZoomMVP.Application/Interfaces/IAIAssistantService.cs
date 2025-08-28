using LegalZoomMVP.Application.DTOs;

namespace LegalZoomMVP.Application.Interfaces
{
    public interface IAIAssistantService
    {
        Task<AIConversationDto> CreateConversationAsync(int userId, CreateAIConversationDto request);
        Task<IEnumerable<AIConversationDto>> GetUserConversationsAsync(int userId);
        Task<AIConversationDto?> GetConversationAsync(int userId, int conversationId);
        Task<AIMessageDto> SendMessageAsync(int userId, int conversationId, SendAIMessageDto request);
        Task<bool> DeleteConversationAsync(int userId, int conversationId);
    }
}