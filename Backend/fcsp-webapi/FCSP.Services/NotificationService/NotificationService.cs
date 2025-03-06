using FCSP.DTOs.Notification;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FCSP.Services.NotificationService
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<IEnumerable<GetNotificationByIdResponse>> GetAllNotifications()
        {
            var notifications = await _notificationRepository.GetAllAsync();
            return notifications.Select(n => MapToResponse(n));
        }

        public async Task<GetNotificationByIdResponse> GetNotificationById(GetNotificationByIdRequest request)
        {
            var notification = await _notificationRepository.FindAsync(request.Id);
            if (notification == null)
            {
                throw new InvalidOperationException($"Notification with ID {request.Id} not found");
            }

            return MapToResponse(notification);
        }

        public async Task<IEnumerable<GetNotificationByIdResponse>> GetNotificationsByUser(GetNotificationsByUserRequest request)
        {
            var notifications = await _notificationRepository.GetNotificationsByUserIdAsync(request.UserId);
            return notifications.Select(n => MapToResponse(n));
        }

        public async Task<AddNotificationResponse> AddNotification(AddNotificationRequest request)
        {
            var notification = new Notification
            {
                UserId = request.UserId,
                Title = request.Title,
                Content = request.Content,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var addedNotification = await _notificationRepository.AddAsync(notification);
            return new AddNotificationResponse { NotificationId = addedNotification.Id };
        }

        public async Task<UpdateNotificationResponse> UpdateNotification(UpdateNotificationRequest request)
        {
            var notification = await _notificationRepository.FindAsync(request.Id);
            if (notification == null)
            {
                throw new InvalidOperationException($"Notification with ID {request.Id} not found");
            }

            notification.Title = request.Title;
            notification.Content = request.Content;
            notification.UpdatedAt = DateTime.UtcNow;

            await _notificationRepository.UpdateAsync(notification);
            return new UpdateNotificationResponse { NotificationId = notification.Id };
        }

        public async Task<DeleteNotificationResponse> DeleteNotification(DeleteNotificationRequest request)
        {
            var notification = await _notificationRepository.FindAsync(request.Id);
            if (notification == null)
            {
                throw new InvalidOperationException($"Notification with ID {request.Id} not found");
            }

            await _notificationRepository.DeleteAsync(request.Id);
            return new DeleteNotificationResponse { Success = true };
        }

        private GetNotificationByIdResponse MapToResponse(Notification notification)
        {
            return new GetNotificationByIdResponse
            {
                Id = notification.Id,
                UserId = notification.UserId,
                Title = notification.Title,
                Content = notification.Content,
                CreatedAt = notification.CreatedAt
            };
        }
    }
} 