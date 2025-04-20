using FCSP.DTOs.Post;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using System;
using System.Linq;

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
            return posts.Select(p => MapToDetailedResponse(p));
        }

        public async Task<GetPostByIdResponse> GetPostById(GetPostByIdRequest request)
        {
            var post = await GetEntityFromGetByIdRequest(request);
            return MapToDetailedResponse(post);
        }

        public async Task<IEnumerable<GetPostByIdResponse>> GetPostsByUser(GetPostsByUserRequest request)
        {
            var posts = await _postRepository.GetPostsByUserIdAsync(request.UserId);
            return posts.Select(p => MapToDetailedResponse(p));
        }

        public async Task<AddPostResponse> AddPost(AddPostRequest request)
        {
            var post = new Posts
            {
                UserId = request.UserId,
                Title = request.Title,
                Content = request.Content,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var addedPost = await _postRepository.AddAsync(post);

            return new AddPostResponse
            {
                Id = addedPost.Id,
                Title = addedPost.Title,
                Content = addedPost.Content,
                CreatedAt = addedPost.CreatedAt
            };
        }

        public async Task<GetPostByIdResponse> UpdatePost(UpdatePostRequest request)
        {
            var post = await GetEntityFromUpdateRequest(request);

            post.Title = request.Title;
            post.Content = request.Content;
            post.UpdatedAt = DateTime.UtcNow;

            await _postRepository.UpdateAsync(post);

            return MapToDetailedResponse(post);
        }

        public async Task<GetPostByIdResponse> DeletePost(DeletePostRequest request)
        {
            var post = await GetEntityFromGetByIdRequest(new GetPostByIdRequest { Id = request.Id });

            post.IsDeleted = true;
            post.UpdatedAt = DateTime.UtcNow;
            await _postRepository.UpdateAsync(post);

            return MapToDetailedResponse(post);
        }

        private GetPostByIdResponse MapToDetailedResponse(Posts post)
        {
            return new GetPostByIdResponse
            {
                Id = post.Id,
                UserId = post.UserId,
                Title = post.Title,
                Content = post.Content,
                ImageUrl = null,
                IsDeleted = post.IsDeleted,
                UserName = post.User?.Name ?? string.Empty,
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdatedAt
            };
        }

        private async Task<Posts> GetEntityFromGetByIdRequest(GetPostByIdRequest request)
        {
            var post = await _postRepository.FindAsync(request.Id);
            if (post == null)
            {
                throw new InvalidOperationException($"Post with ID {request.Id} not found");
            }
            return post;
        }

        private async Task<Posts> GetEntityFromUpdateRequest(UpdatePostRequest request)
        {
            var post = await _postRepository.FindAsync(request.Id);
            if (post == null)
            {
                throw new InvalidOperationException($"Post with ID {request.Id} not found");
            }
            return post;
        }
    }
}