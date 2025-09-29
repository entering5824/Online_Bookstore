using Online_Bookstore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Online_Bookstore.Services.Interfaces
{
    public interface IActivityLogService
    {
        Task<List<ActivityLog>> GetAllActivityLogsAsync();
        Task<ActivityLog> GetActivityLogByIdAsync(int id);
        Task SaveActivityLogAsync(ActivityLog log);
        Task DeleteActivityLogAsync(int id);
    }
}