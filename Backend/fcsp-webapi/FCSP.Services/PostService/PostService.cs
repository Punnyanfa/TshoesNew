using FCSP.DTOs.Post;
using FCSP.Models.Entities;
using FCSP.Repositories.Post;
using Microsoft.EntityFrameworkCore;

namespace FCSP.Services.PostService
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;

        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<IEnumerable<GetPostByIdResponse>> GetAllPosts()
        {
            var posts = await _postRepository.GetAllAsync();
            return posts.Select(MapToDetailedResponse);
        }

        public async Task<GetPostByIdResponse> GetPostById(GetPostByIdRequest request)
        {
            Post post = GetEntityFromGetByIdRequest(request);
            return MapToDetailedResponse(post);
        }

        public async Task<IEnumerable<GetPostByIdResponse>> GetPostsByUser(GetPostsByUserRequest request)
        {
            var posts = await _postRepository.GetPostsByUserIdAsync(request.UserId);
            return posts.Select(MapToDetailedResponse);
        }

        public async Task<AddPostResponse> AddPost(AddPostRequest request)
        {
            Post post = GetEntityFromAddRequest(request);
            var addedPost = await _postRepository.AddAsync(post);
            
            return new AddPostResponse
            {
                Id = addedPost.Id,
                UserId = addedPost.UserId,
                Title = addedPost.Title,
                Description = addedPost.Description,
                CreatedAt = addedPost.CreatedAt
            };
        }

        public async Task<GetPostByIdResponse> UpdatePost(UpdatePostRequest request)
        {
            Post post = GetEntityFromUpdateRequest(request);
            await _postRepository.UpdateAsync(post);
            return MapToDetailedResponse(post);
        }

        public async Task<GetPostByIdResponse> DeletePost(DeletePostRequest request)
        {
            Post post = GetEntityFromDeleteRequest(request);
            var response = MapToDetailedResponse(post);
            await _postRepository.DeleteAsync(post.Id);
            return response;
        }

        private GetPostByIdResponse MapToDetailedResponse(Post post)
        {
            return new GetPostByIdResponse
            {
                Id = post.Id,
                UserId = post.UserId,
                Title = post.Title,
                Description = post.Description,
                UserName = post.User?.Name,
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdatedAt,
                CommentCount = post.PostComments?.Count() ?? 0
            };
        }

        private Post GetEntityFromGetByIdRequest(GetPostByIdRequest request)
        {
            Post post = _postRepository.Find(request.Id);
            if (post == null)
            {
                throw new InvalidOperationException("Post not found");
            }
            return post;
        }

        private Post GetEntityFromAddRequest(AddPostRequest request)
        {
            return new Post
            {
                UserId = request.UserId,
                Title = request.Title,
                Description = request.Description
            };
        }

        private Post GetEntityFromUpdateRequest(UpdatePostRequest request)
        {
            Post post = _postRepository.Find(request.Id);
            if (post == null)
            {
                throw new InvalidOperationException("Post not found");
            }
            
            post.Title = request.Title ?? post.Title;
            post.Description = request.Description ?? post.Description;
            post.UpdatedAt = DateTime.Now;
            
            return post;
        }

        private Post GetEntityFromDeleteRequest(DeletePostRequest request)
        {
            Post post = _postRepository.Find(request.Id);
            if (post == null)
            {
                throw new InvalidOperationException("Post not found");
            }
            return post;
        }
    }
} 