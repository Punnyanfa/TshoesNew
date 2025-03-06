using System;

namespace FCSP.DTOs.Notification
{
    public class GetNotificationByIdRequest
    {
        public long Id { get; set; }
    }

    public class GetNotificationByIdResponse
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class GetNotificationsByUserRequest
    {
        public long UserId { get; set; }
    }

    public class AddNotificationRequest
    {
        public long UserId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }

    public class AddNotificationResponse
    {
        public long NotificationId { get; set; }
    }

    public class UpdateNotificationRequest
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }

    public class UpdateNotificationResponse
    {
        public long NotificationId { get; set; }
    }

    public class DeleteNotificationRequest
    {
        public long Id { get; set; }
    }

    public class DeleteNotificationResponse
    {
        public bool Success { get; set; }
    }
}

