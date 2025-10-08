using Online_Bookstore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Bookstore.Repository
{
    public class UserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllAsync()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("🔍 UserRepository.GetAllAsync: Starting...");
                var result = await _context.Users.ToListAsync();
                System.Diagnostics.Debug.WriteLine($"✅ UserRepository.GetAllAsync: Successfully loaded {result.Count} users");
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("❌ Lỗi trong UserRepository.GetAllAsync: " + ex.Message);
                if (ex.InnerException != null)
                    System.Diagnostics.Debug.WriteLine("Inner: " + ex.InnerException.Message);
                throw;
            }
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task SaveAsync(User user)
        {
            if (user.Id == 0)
            {
                _context.Users.Add(user);
            }
            else
            {
                _context.Entry(user).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
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
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
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
                .GroupBy(u => new { Year = u.CreatedDate.Year, Month = u.CreatedDate.Month })
                .Select(g => new {
                    Month = (g.Key.Month + "/" + g.Key.Year),
                    Count = g.LongCount()
                })
                .ToDictionaryAsync(x => x.Month, x => x.Count);
        }
    }
}