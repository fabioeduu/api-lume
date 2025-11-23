using Lume.Api.Data;
using Lume.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Lume.Api.Repositories
{
    public interface ICheckinRepository : IRepository<Checkin>
    {
        Task<IEnumerable<Checkin>> GetUserCheckinsAsync(int userId);
        Task<IEnumerable<Checkin>> GetUserCheckinsAsync(int userId, DateTime fromDate, DateTime toDate);
    }

    public class CheckinRepository : Repository<Checkin>, ICheckinRepository
    {
        private readonly LumeContext _context;

        public CheckinRepository(LumeContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Checkin>> GetUserCheckinsAsync(int userId)
        {
            return await _context.Checkins
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Checkin>> GetUserCheckinsAsync(int userId, DateTime fromDate, DateTime toDate)
        {
            return await _context.Checkins
                .Where(c => c.UserId == userId && c.CreatedAt >= fromDate && c.CreatedAt <= toDate)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }
    }
}
