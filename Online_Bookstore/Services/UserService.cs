using Online_Bookstore.Models;
using Online_Bookstore.Services.Interfaces;
using Online_Bookstore.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;  // chú ý namespace của EF6
using System.Linq;
using System.Threading.Tasks;

namespace Online_Bookstore.Services
{
    public class UserService : IUserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("🔍 UserService.GetAllUsersAsync: Starting...");
                var result = await _userRepository.GetAllAsync();
                System.Diagnostics.Debug.WriteLine($"✅ UserService.GetAllUsersAsync: Successfully loaded {result.Count} users");
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("❌ Lỗi trong GetAllUsersAsync: " + ex.Message);
                if (ex.InnerException != null)
                    System.Diagnostics.Debug.WriteLine("Inner: " + ex.InnerException.Message);
                throw;
            }
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task SaveUserAsync(User user)
        {
            await _userRepository.SaveAsync(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            await _userRepository.DeleteAsync(id);
        }

        public async Task<User> FindByUsernameAsync(string username)
        {
            return await _userRepository.FindByUsernameAsync(username);
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return await _userRepository.FindByEmailAsync(email);
        }

        public async Task<Dictionary<string, long>> GetUsersByRoleAsync()
        {
            return await _userRepository.GetUsersByRoleAsync();
        }

        public async Task<Dictionary<string, long>> GetUserRegistrationByMonthAsync()
        {
            return await _userRepository.GetUserRegistrationByMonthAsync();
        }
    }
}
