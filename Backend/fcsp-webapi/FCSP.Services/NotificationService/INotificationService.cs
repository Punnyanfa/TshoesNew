using FCSP.DTOs.Notification;

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