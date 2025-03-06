namespace FCSP.DTOs.PostsComments
{
    public class GetPostsCommentByIdRequest
    {
        public long Id { get; set; }
    }

    public class GetPostsCommentByIdResponse
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long PostsId { get; set; }
        public string Comment { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }

    public class GetCommentsByPostRequest
    {
        public long PostId { get; set; }
    }

    public class GetCommentsByUserRequest
    {
        public long UserId { get; set; }
    }

    public class AddPostsCommentRequest
    {
        public long UserId { get; set; }
        public long PostsId { get; set; }
        public string Comment { get; set; } = null!;
    }

    public class AddPostsCommentResponse
    {
        public long PostsCommentId { get; set; }
    }

    public class UpdatePostsCommentRequest
    {
        public long Id { get; set; }
        public string Comment { get; set; } = null!;
    }

    public class UpdatePostsCommentResponse
    {
        public long PostsCommentId { get; set; }
    }

    public class DeletePostsCommentRequest
    {
        public long Id { get; set; }
    }

    public class DeletePostsCommentResponse
    {
        public bool Success { get; set; }
    }
} 