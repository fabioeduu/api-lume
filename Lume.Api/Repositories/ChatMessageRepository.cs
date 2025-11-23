using Lume.Api.Data;
using Lume.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Lume.Api.Repositories
{
    public interface IChatMessageRepository : IRepository<ChatMessage>
    {
        Task<IEnumerable<ChatMessage>> GetUserChatHistoryAsync(int userId);
        Task<IEnumerable<ChatMessage>> GetUserChatHistoryAsync(int userId, int limit);
    }

    public class ChatMessageRepository : Repository<ChatMessage>, IChatMessageRepository
    {
        private readonly LumeContext _context;

        public ChatMessageRepository(LumeContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ChatMessage>> GetUserChatHistoryAsync(int userId)
        {
            return await _context.ChatMessages
                .Where(cm => cm.UserId == userId)
                .OrderBy(cm => cm.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<ChatMessage>> GetUserChatHistoryAsync(int userId, int limit)
        {
            return await _context.ChatMessages
                .Where(cm => cm.UserId == userId)
                .OrderByDescending(cm => cm.CreatedAt)
                .Take(limit)
                .OrderBy(cm => cm.CreatedAt)
                .ToListAsync();
        }
    }
}
