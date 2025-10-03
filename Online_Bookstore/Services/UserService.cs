using Online_Bookstore.Models;
using Online_Bookstore.Services.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;  // chú ý namespace của EF6
using System.Linq;
using System.Threading.Tasks;

namespace Online_Bookstore.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task SaveUserAsync(User user)
        {
            if (user.UserId == 0)
            {
                _context.Users.Add(user);
            }
            else
            {
                _context.Entry(user).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<User> FindByUsernameAsync(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<Dictionary<string, long>> GetUsersByRoleAsync()
        {
            return await _context.Users
                .GroupBy(u => u.Role)
                .Select(g => new { Role = g.Key, Count = g.LongCount() })
                .ToDictionaryAsync(x => x.Role, x => x.Count);
        }

        public async Task<Dictionary<string, long>> GetUserRegistrationByMonthAsync()
        {
            return await _context.Users
                .Where(u => u.CreatedAt.HasValue)
                .GroupBy(u => new { Year = u.CreatedAt.Value.Year, Month = u.CreatedAt.Value.Month })
                .Select(g => new {
                    Month = (g.Key.Month + "/" + g.Key.Year),
                    Count = g.LongCount()
                })
                .ToDictionaryAsync(x => x.Month, x => x.Count);
        }
    }
}
