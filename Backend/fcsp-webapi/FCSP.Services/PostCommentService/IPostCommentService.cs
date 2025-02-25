using FCSP.DTOs.PostComment;
using FCSP.Models.Entities;

namespace FCSP.Services.PostCommentService
{
    public interface IPostCommentService
    {
        Task<IEnumerable<GetPostCommentByIdResponse>> GetAllPostComments();
        Task<GetPostCommentByIdResponse> GetPostCommentById(GetPostCommentByIdRequest request);
        Task<IEnumerable<GetPostCommentByIdResponse>> GetPostCommentsByPost(GetPostCommentsByPostRequest request);
        Task<IEnumerable<GetPostCommentByIdResponse>> GetPostCommentsByUser(GetPostCommentsByUserRequest request);
        Task<AddPostCommentResponse> AddPostComment(AddPostCommentRequest request);
        Task<GetPostCommentByIdResponse> UpdatePostComment(UpdatePostCommentRequest request);
        Task<GetPostCommentByIdResponse> DeletePostComment(DeletePostCommentRequest request);
    }
} 