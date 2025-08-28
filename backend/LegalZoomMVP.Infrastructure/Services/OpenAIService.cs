using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using LegalZoomMVP.Application.DTOs;

namespace LegalZoomMVP.Infrastructure.Services
{
    public class OpenAIService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public OpenAIService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            
            _httpClient.BaseAddress = new Uri("https://api.openai.com/");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_configuration["OpenAI:ApiKey"]}");
        }

        public async Task<string> GetLegalAdviceAsync(string userMessage, List<AIMessageDto> conversationHistory, Dictionary<string, object>? userFormData = null)
        {
            var messages = new List<object>
            {
                new
                {
                    role = "system",
                    content = GetSystemPrompt(userFormData)
                }
            };

            // Add conversation history
            foreach (var message in conversationHistory.TakeLast(10)) // Limit to last 10 messages
            {
                messages.Add(new
                {
                    role = message.Role.ToLower(),
                    content = message.Content
                });
            }

            // Add current user message
            messages.Add(new
            {
                role = "user",
                content = userMessage
            });

            var requestBody = new
            {
                model = "gpt-4",
                messages = messages,
                max_tokens = 1000,
                temperature = 0.7
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("v1/chat/completions", content);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();
            var openAIResponse = JsonSerializer.Deserialize<OpenAIResponse>(responseJson);

            return openAIResponse?.Choices?.FirstOrDefault()?.Message?.Content ?? "I apologize, but I'm having trouble generating a response right now.";
        }

        private string GetSystemPrompt(Dictionary<string, object>? userFormData)
        {
            var basePrompt = @"You are a knowledgeable legal assistant helping users with general legal questions and document preparation. 

IMPORTANT DISCLAIMERS:
- You provide general legal information, not legal advice
- Users should consult with qualified attorneys for specific legal matters
- Your responses are for informational purposes only
- Laws vary by jurisdiction

Your role is to:
1. Answer general legal questions clearly and accurately
2. Help users understand legal concepts and procedures
3. Provide guidance on document preparation and legal forms
4. Reference relevant laws and regulations when appropriate
5. Always remind users to seek professional legal counsel for specific situations

Be professional, accurate, and helpful while staying within appropriate bounds.";

            if (userFormData != null && userFormData.Any())
            {
                var formContext = string.Join(", ", userFormData.Select(kvp => $"{kvp.Key}: {kvp.Value}"));
                basePrompt += $"\n\nUser's form data context: {formContext}";
                basePrompt += "\nYou can reference this information when providing relevant advice about their legal documents.";
            }

            return basePrompt;
        }

        private class OpenAIResponse
        {
            public List<Choice>? Choices { get; set; }
        }

        private class Choice
        {
            public Message? Message { get; set; }
        }

        private class Message
        {
            public string? Content { get; set; }
        }
    }
}