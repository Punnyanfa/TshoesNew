using FCSP.DTOs.PostsComments;
using FCSP.Models.Entities;

namespace FCSP.Services.PostsCommentsService
{
    public interface IPostsCommentsService
    {
        Task<IEnumerable<GetPostsCommentByIdResponse>> GetAllPostsComments();
        Task<GetPostsCommentByIdResponse> GetPostsCommentById(GetPostsCommentByIdRequest request);
        Task<IEnumerable<GetPostsCommentByIdResponse>> GetCommentsByPost(GetCommentsByPostRequest request);
        Task<IEnumerable<GetPostsCommentByIdResponse>> GetCommentsByUser(GetCommentsByUserRequest request);
        Task<AddPostsCommentResponse> AddPostsComment(AddPostsCommentRequest request);
        Task<UpdatePostsCommentResponse> UpdatePostsComment(UpdatePostsCommentRequest request);
        Task<DeletePostsCommentResponse> DeletePostsComment(DeletePostsCommentRequest request);
    }
} 