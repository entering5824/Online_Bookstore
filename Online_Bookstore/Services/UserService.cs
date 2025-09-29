using Online_Bookstore.Models;
using Online_Bookstore.Repository;
using Online_Bookstore.Services.Interfaces;

using System;
using System.Collections.Generic;
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
            return await _userRepository.GetAllAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new Exception($"Không tìm thấy user với ID: {id}");
            return user;
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
            var users = await GetAllUsersAsync();
            return users
                .GroupBy(u => string.IsNullOrEmpty(u.Role) ? "Không xác định" : u.Role)
                .ToDictionary(g => g.Key, g => (long)g.Count());
        }

        public async Task<Dictionary<string, long>> GetUserRegistrationByMonthAsync()
        {
            var users = await GetAllUsersAsync();
            return users
                .Where(u => u.CreatedAt.HasValue)
                .GroupBy(u => u.CreatedAt.Value.ToString("yyyy-MM"))
                .ToDictionary(g => g.Key, g => (long)g.Count());
        }
    }
}