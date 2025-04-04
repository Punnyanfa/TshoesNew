using FCSP.DTOs;
using FCSP.DTOs.Notification;
using FCSP.Models.Entities;

namespace FCSP.Services.NotificationService
{
    public interface INotificationService
    {
        Task<IEnumerable<GetNotificationByIdResponse>> GetAllNotifications();
        Task<GetNotificationByIdResponse> GetNotificationById(GetNotificationByIdRequest request);
        Task<IEnumerable<GetNotificationByIdResponse>> GetNotificationsByUser(GetNotificationsByUserRequest request);
        Task<AddNotificationResponse> AddNotification(AddNotificationRequest request);
        Task<UpdateNotificationResponse> UpdateNotification(UpdateNotificationRequest request);
        Task<DeleteNotificationResponse> DeleteNotification(DeleteNotificationRequest request);
    }
} 