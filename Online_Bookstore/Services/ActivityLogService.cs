using Online_Bookstore.Models;
using Online_Bookstore.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Online_Bookstore.Services
{
    public class ActivityLogService
    {
        private readonly ActivityLogRepository _activityLogRepository;

        public ActivityLogService(ActivityLogRepository activityLogRepository)
        {
            _activityLogRepository = activityLogRepository;
        }

        public async Task<List<ActivityLog>> GetAllActivityLogsAsync()
        {
            return await _activityLogRepository.GetAllAsync();
        }

        public async Task<ActivityLog> GetActivityLogByIdAsync(int id)
        {
            return await _activityLogRepository.GetByIdAsync(id);
        }

        public async Task SaveActivityLogAsync(ActivityLog log)
        {
            await _activityLogRepository.SaveAsync(log);
        }

        public async Task DeleteActivityLogAsync(int id)
        {
            await _activityLogRepository.DeleteAsync(id);
        }
    }
}