using FCSP.DTOs.Post;

namespace FCSP.Services.PostService
{
    public interface IPostService
    {
        Task<IEnumerable<GetPostByIdResponse>> GetAllPosts();
        Task<GetPostByIdResponse> GetPostById(GetPostByIdRequest request);
        Task<IEnumerable<GetPostByIdResponse>> GetPostsByUser(GetPostsByUserRequest request);
        Task<AddPostResponse> AddPost(AddPostRequest request);
        Task<GetPostByIdResponse> UpdatePost(UpdatePostRequest request);
        Task<GetPostByIdResponse> DeletePost(DeletePostRequest request);
    }
}