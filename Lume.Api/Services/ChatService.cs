using Lume.Api.DTOs;
using Lume.Api.Models;
using Lume.Api.Repositories;

namespace Lume.Api.Services
{
    public interface IChatService
    {
        Task<ChatMessageDto?> SendMessageAsync(int userId, CreateChatMessageDto messageDto);
        Task<IEnumerable<ChatMessageDto>> GetChatHistoryAsync(int userId);
        Task<IEnumerable<ChatMessageDto>> GetChatHistoryAsync(int userId, int limit);
        Task<bool> DeleteChatMessageAsync(int messageId);
    }

    public class ChatService : IChatService
    {
        private readonly IChatMessageRepository _chatRepository;

        public ChatService(IChatMessageRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public async Task<ChatMessageDto?> SendMessageAsync(int userId, CreateChatMessageDto messageDto)
        {
            var userMessage = new ChatMessage
            {
                UserId = userId,
                SenderType = "user",
                Message = messageDto.Message,
                CreatedAt = DateTime.UtcNow
            };

            var savedMessage = await _chatRepository.AddAsync(userMessage);

            // Generate system response (basic logic)
            var systemResponse = await GenerateSystemResponseAsync(userId, messageDto.Message);

            return new ChatMessageDto
            {
                Id = savedMessage.Id,
                UserId = savedMessage.UserId,
                SenderType = savedMessage.SenderType,
                Message = savedMessage.Message,
                CreatedAt = savedMessage.CreatedAt
            };
        }

        public async Task<IEnumerable<ChatMessageDto>> GetChatHistoryAsync(int userId)
        {
            var messages = await _chatRepository.GetUserChatHistoryAsync(userId);
            return messages.Select(m => new ChatMessageDto
            {
                Id = m.Id,
                UserId = m.UserId,
                SenderType = m.SenderType,
                Message = m.Message,
                CreatedAt = m.CreatedAt
            });
        }

        public async Task<IEnumerable<ChatMessageDto>> GetChatHistoryAsync(int userId, int limit)
        {
            var messages = await _chatRepository.GetUserChatHistoryAsync(userId, limit);
            return messages.Select(m => new ChatMessageDto
            {
                Id = m.Id,
                UserId = m.UserId,
                SenderType = m.SenderType,
                Message = m.Message,
                CreatedAt = m.CreatedAt
            });
        }

        public async Task<bool> DeleteChatMessageAsync(int messageId)
        {
            return await _chatRepository.DeleteAsync(messageId);
        }

        private async Task<ChatMessage> GenerateSystemResponseAsync(int userId, string userMessage)
        {
            // Simple response logic - can be enhanced with AI/ML in future
            var response = GetEmotionalSupport(userMessage);

            var systemMessage = new ChatMessage
            {
                UserId = userId,
                SenderType = "system",
                Message = response,
                CreatedAt = DateTime.UtcNow
            };

            return await _chatRepository.AddAsync(systemMessage);
        }

        private string GetEmotionalSupport(string userMessage)
        {
            // Simple logic for emotional support responses
            var lowerMessage = userMessage.ToLower();

            if (lowerMessage.Contains("sad") || lowerMessage.Contains("triste"))
                return "Lamento ouvir que você está se sentindo triste. Gostaria de conversar sobre o que está deixando você assim? Lembre-se que você não está sozinho.";

            if (lowerMessage.Contains("angry") || lowerMessage.Contains("raiva") || lowerMessage.Contains("irritado"))
                return "Entendo sua frustração. Às vezes é importante respirar fundo e tirar alguns momentos para si mesmo. O que você gostaria de fazer para se acalmar?";

            if (lowerMessage.Contains("anxious") || lowerMessage.Contains("ansioso") || lowerMessage.Contains("preocupado"))
                return "A ansiedade é normal, mas você tem capacidade de lidar com isso. Tente praticar respiração profunda e focar no presente.";

            if (lowerMessage.Contains("happy") || lowerMessage.Contains("feliz") || lowerMessage.Contains("alegre"))
                return "Que maravilhoso saber que você está se sentindo bem! Continue celebrando esses momentos positivos.";

            return "Obrigado por compartilhar seus sentimentos. Estou aqui para ouvi-lo. Como você está se sentindo?";
        }
    }
}
