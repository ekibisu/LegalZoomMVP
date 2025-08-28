using Microsoft.EntityFrameworkCore;
using LegalZoomMVP.Application.DTOs;
using LegalZoomMVP.Application.Interfaces;
using LegalZoomMVP.Application.Exceptions;
using LegalZoomMVP.Domain.Entities;
using LegalZoomMVP.Infrastructure.Data;
using LegalZoomMVP.Infrastructure.Services;
using System.Text.Json;

namespace LegalZoomMVP.Infrastructure.Services
{
    public class AIAssistantService : IAIAssistantService
    {
        private readonly ApplicationDbContext _context;
        private readonly OpenAIService _openAIService;

        public AIAssistantService(ApplicationDbContext context, OpenAIService openAIService)
        {
            _context = context;
            _openAIService = openAIService;
        }

        public async Task<AIConversationDto> CreateConversationAsync(int userId, CreateAIConversationDto request)
        {
            var conversation = new AIConversation
            {
                UserId = userId,
                Title = request.Title
            };

            _context.AIConversations.Add(conversation);
            await _context.SaveChangesAsync();

            // Send initial message and get AI response
            var aiResponse = await SendMessageAsync(userId, conversation.Id, new SendAIMessageDto { Message = request.InitialMessage });

            return new AIConversationDto
            {
                Id = conversation.Id,
                Title = conversation.Title,
                CreatedAt = conversation.CreatedAt,
                Messages = new List<AIMessageDto> { aiResponse }
            };
        }

        public async Task<IEnumerable<AIConversationDto>> GetUserConversationsAsync(int userId)
        {
            var conversations = await _context.AIConversations
                .Include(c => c.Messages)
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.UpdatedAt ?? c.CreatedAt)
                .ToListAsync();

            return conversations.Select(c => new AIConversationDto
            {
                Id = c.Id,
                Title = c.Title,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
                Messages = c.Messages.OrderBy(m => m.CreatedAt).Select(m => new AIMessageDto
                {
                    Id = m.Id,
                    Content = m.Content,
                    Role = m.Role.ToString(),
                    CreatedAt = m.CreatedAt
                }).ToList()
            });
        }

        public async Task<AIConversationDto?> GetConversationAsync(int userId, int conversationId)
        {
            var conversation = await _context.AIConversations
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.Id == conversationId && c.UserId == userId);

            if (conversation == null) return null;

            return new AIConversationDto
            {
                Id = conversation.Id,
                Title = conversation.Title,
                CreatedAt = conversation.CreatedAt,
                UpdatedAt = conversation.UpdatedAt,
                Messages = conversation.Messages.OrderBy(m => m.CreatedAt).Select(m => new AIMessageDto
                {
                    Id = m.Id,
                    Content = m.Content,
                    Role = m.Role.ToString(),
                    CreatedAt = m.CreatedAt
                }).ToList()
            };
        }

        public async Task<AIMessageDto> SendMessageAsync(int userId, int conversationId, SendAIMessageDto request)
        {
            var conversation = await _context.AIConversations
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.Id == conversationId && c.UserId == userId);

            if (conversation == null)
                throw new NotFoundException("Conversation not found");

            // Add user message
            var userMessage = new AIMessage
            {
                ConversationId = conversationId,
                Content = request.Message,
                Role = MessageRole.User
            };

            _context.AIMessages.Add(userMessage);

            // Get user's form data for context
            var userFormData = await GetUserFormDataForContext(userId);

            // Get AI response
            var conversationHistory = conversation.Messages.Select(m => new AIMessageDto
            {
                Content = m.Content,
                Role = m.Role.ToString()
            }).ToList();

            var aiResponseContent = await _openAIService.GetLegalAdviceAsync(request.Message, conversationHistory, userFormData);

            // Add AI response message
            var aiMessage = new AIMessage
            {
                ConversationId = conversationId,
                Content = aiResponseContent,
                Role = MessageRole.Assistant
            };

            _context.AIMessages.Add(aiMessage);

            // Update conversation timestamp
            conversation.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new AIMessageDto
            {
                Id = aiMessage.Id,
                Content = aiMessage.Content,
                Role = aiMessage.Role.ToString(),
                CreatedAt = aiMessage.CreatedAt
            };
        }

        public async Task<bool> DeleteConversationAsync(int userId, int conversationId)
        {
            var conversation = await _context.AIConversations
                .FirstOrDefaultAsync(c => c.Id == conversationId && c.UserId == userId);

            if (conversation == null) return false;

            _context.AIConversations.Remove(conversation);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<Dictionary<string, object>?> GetUserFormDataForContext(int userId)
        {
            // Get the most recent completed form for context
            var recentForm = await _context.UserForms
                .Include(uf => uf.FormTemplate)
                .Where(uf => uf.UserId == userId && uf.Status == FormStatus.Completed)
                .OrderByDescending(uf => uf.CompletedAt)
                .FirstOrDefaultAsync();

            if (recentForm == null) return null;

            try
            {
                var formData = JsonSerializer.Deserialize<Dictionary<string, object>>(recentForm.FormData);
                formData?.Add("FormType", recentForm.FormTemplate.Name);
                return formData;
            }
            catch
            {
                return null;
            }
        }
    }
}