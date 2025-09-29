using Online_Bookstore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Online_Bookstore.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task SaveUserAsync(User user);
        Task DeleteUserAsync(int id);
        Task<User> FindByUsernameAsync(string username);
        Task<User> FindByEmailAsync(string email);
        Task<Dictionary<string, long>> GetUsersByRoleAsync();
        Task<Dictionary<string, long>> GetUserRegistrationByMonthAsync();
    }
}