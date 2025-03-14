namespace FCSP.DTOs.Post
{
    public class GetPostByIdRequest
    {
        public long Id { get; set; }
    }

    public class GetPostByIdResponse
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public bool IsDeleted { get; set; }
        public string UserName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class AddPostRequest
    {
        public long UserId { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public int Status { get; set; }
    }

    public class AddPostResponse
    {
        public long Id { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }

    public class UpdatePostRequest
    {
        public long Id { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public int Status { get; set; }
    }

    public class UpdatePostResponse
    {
        public long Id { get; set; }
        public string Title { get; set; } = null!;
        public DateTime UpdatedAt { get; set; }
    }

    public class DeletePostRequest
    {
        public long Id { get; set; }
    }

    public class DeletePostResponse
    {
        public bool Success { get; set; }
    }

    public class GetPostsByUserRequest
    {
        public long UserId { get; set; }
    }
} 