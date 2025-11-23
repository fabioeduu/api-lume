using Lume.Api.Data;
using Lume.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Lume.Api.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
    }

    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly LumeContext _context;

        public UserRepository(LumeContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
