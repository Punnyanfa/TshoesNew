namespace FCSP.DTOs.UserActivity
{
    public class GetUserActivityByIdRequest
    {
        public long Id { get; set; }
    }

    public class GetUserActivityByIdResponse
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long ViewedDesignId { get; set; }
        public string ViewAt { get; set; } = null!;
        public int ViewDuration { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class GetActivitiesByUserRequest
    {
        public long UserId { get; set; }
    }

    public class GetActivitiesByDesignRequest
    {
        public long DesignId { get; set; }
    }

    public class AddUserActivityRequest
    {
        public long UserId { get; set; }
        public long ViewedDesignId { get; set; }
        public string ViewAt { get; set; } = null!;
        public int ViewDuration { get; set; }
    }

    public class AddUserActivityResponse
    {
        public long UserActivityId { get; set; }
    }

    public class UpdateUserActivityRequest
    {
        public long Id { get; set; }
        public string ViewAt { get; set; } = null!;
        public int ViewDuration { get; set; }
    }

    public class UpdateUserActivityResponse
    {
        public long UserActivityId { get; set; }
    }

    public class DeleteUserActivityRequest
    {
        public long Id { get; set; }
    }

    public class DeleteUserActivityResponse
    {
        public bool Success { get; set; }
    }
} 