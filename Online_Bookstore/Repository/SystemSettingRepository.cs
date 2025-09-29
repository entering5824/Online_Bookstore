using Online_Bookstore.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Bookstore.Repository
{
    public class SystemSettingRepository
    {
        private readonly ApplicationDbContext _context;

        public SystemSettingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<SystemSetting>> GetAllAsync()
        {
            return await _context.SystemSettings.ToListAsync();
        }

        public async Task<SystemSetting> GetByKeyAsync(string key)
        {
            return await _context.SystemSettings.FirstOrDefaultAsync(s => s.SettingKey == key);
        }

        public async Task SaveAsync(SystemSetting setting)
        {
            var existing = await GetByKeyAsync(setting.SettingKey);
            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(setting);
            }
            else
            {
                _context.SystemSettings.Add(setting);
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string key)
        {
            var setting = await _context.SystemSettings.FirstOrDefaultAsync(s => s.SettingKey == key);
            if (setting != null)
            {
                _context.SystemSettings.Remove(setting);
                await _context.SaveChangesAsync();
            }
        }
    }
}