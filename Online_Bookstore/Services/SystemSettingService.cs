using Online_Bookstore.Models;
using Online_Bookstore.Repository;
using Online_Bookstore.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Online_Bookstore.Services
{
    public class SystemSettingService : ISystemSettingService
    {
        private readonly SystemSettingRepository _settingRepository;

        public SystemSettingService(SystemSettingRepository settingRepository)
        {
            _settingRepository = settingRepository;
        }

        public async Task<List<SystemSetting>> GetAllSettingsAsync()
        {
            return await _settingRepository.GetAllAsync();
        }

        public async Task<SystemSetting> GetSettingByKeyAsync(string key)
        {
            var setting = await _settingRepository.GetByKeyAsync(key);
            if (setting == null)
                throw new Exception($"Không tìm thấy setting với key: {key}");
            return setting;
        }

        public async Task SaveSettingAsync(SystemSetting setting)
        {
            await _settingRepository.SaveAsync(setting);
        }

        public async Task DeleteSettingAsync(string key)
        {
            await _settingRepository.DeleteAsync(key);
        }
    }
}