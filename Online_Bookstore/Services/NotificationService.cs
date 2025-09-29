using Online_Bookstore.Models;
using Online_Bookstore.Repository;
using Online_Bookstore.Services.Interfaces;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Online_Bookstore.Services
{
    public class NotificationService : INotificationService
    {
        private readonly NotificationRepository _notificationRepository;
        private readonly UserRepository _userRepository;

        public NotificationService(NotificationRepository notificationRepository, UserRepository userRepository)
        {
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
        }

        public async Task<List<Notification>> GetAllNotificationsAsync()
        {
            return await _notificationRepository.GetAllAsync();
        }

        public async Task<Notification> GetNotificationByIdAsync(int id)
        {
            var notification = await _notificationRepository.GetByIdAsync(id);
            if (notification == null)
                throw new Exception($"Không tìm thấy thông báo với ID: {id}");
            return notification;
        }

        public async Task SaveNotificationAsync(Notification notification)
        {
            if (notification.UserId.HasValue)
            {
                var user = await _userRepository.GetByIdAsync(notification.UserId.Value);
                if (user == null)
                    throw new Exception($"Không tìm thấy người dùng với ID: {notification.UserId}");
                notification.User = user;
            }

            await _notificationRepository.SaveAsync(notification);
        }

        public async Task DeleteNotificationAsync(int id)
        {
            await _notificationRepository.DeleteAsync(id);
        }
    }
}