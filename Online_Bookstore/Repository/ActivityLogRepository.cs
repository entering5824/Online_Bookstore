using Online_Bookstore.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Bookstore.Repository
{
    public class ActivityLogRepository
    {
        private readonly ApplicationDbContext _context;

        public ActivityLogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ActivityLog>> GetAllAsync()
        {
            return await _context.ActivityLogs.ToListAsync();
        }

        public async Task<ActivityLog> GetByIdAsync(int id)
        {
            return await _context.ActivityLogs.FindAsync(id);
        }

        public async Task SaveAsync(ActivityLog log)
        {
            if (log.LogId == 0)
            {
                _context.ActivityLogs.Add(log);
            }
            else
            {
                _context.Entry(log).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var log = await _context.ActivityLogs.FindAsync(id);
            if (log != null)
            {
                _context.ActivityLogs.Remove(log);
                await _context.SaveChangesAsync();
            }
        }
    }
}