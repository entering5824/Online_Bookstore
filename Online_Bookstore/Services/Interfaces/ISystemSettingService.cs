using Online_Bookstore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Online_Bookstore.Services.Interfaces
{
    public interface ISystemSettingService
    {
        Task<List<SystemSetting>> GetAllSettingsAsync();
        Task<SystemSetting> GetSettingByKeyAsync(string key);
        Task SaveSettingAsync(SystemSetting setting);
        Task DeleteSettingAsync(string key);
    }
}