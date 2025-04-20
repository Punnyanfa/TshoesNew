using FCSP.DTOs.PostsComments;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using System;
using System.Linq;

namespace FCSP.Services.PostsCommentsService
{
    public class PostsCommentsService : IPostsCommentsService
    {
        private readonly IPostsCommentsRepository _postsCommentsRepository;

        public PostsCommentsService(IPostsCommentsRepository postsCommentsRepository)
        {
            _postsCommentsRepository = postsCommentsRepository;
        }

        public async Task<IEnumerable<GetPostsCommentByIdResponse>> GetAllPostsComments()
        {
            var comments = await _postsCommentsRepository.GetAllAsync();
            return comments.Select(c => MapToResponse(c));
        }

        public async Task<GetPostsCommentByIdResponse> GetPostsCommentById(GetPostsCommentByIdRequest request)
        {
            var comment = await _postsCommentsRepository.FindAsync(request.Id);
            if (comment == null)
            {
                throw new InvalidOperationException($"Comment with ID {request.Id} not found");
            }

            return MapToResponse(comment);
        }

        public async Task<IEnumerable<GetPostsCommentByIdResponse>> GetCommentsByPost(GetCommentsByPostRequest request)
        {
            var comments = await _postsCommentsRepository.GetCommentsByPostIdAsync(request.PostId);

            return comments.Select(c => MapToResponse(c));
        }

        public async Task<IEnumerable<GetPostsCommentByIdResponse>> GetCommentsByUser(GetCommentsByUserRequest request)
        {
            var comments = await _postsCommentsRepository.GetCommentsByUserIdAsync(request.UserId);
            return comments.Select(c => MapToResponse(c));
        }

        public async Task<AddPostsCommentResponse> AddPostsComment(AddPostsCommentRequest request)
        {
            var comment = new PostsComments
            {
                UserId = request.UserId,
                PostsId = request.PostsId,
                Comment = request.Comment,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var addedComment = await _postsCommentsRepository.AddAsync(comment);
            return new AddPostsCommentResponse { PostsCommentId = addedComment.Id };
        }

        public async Task<UpdatePostsCommentResponse> UpdatePostsComment(UpdatePostsCommentRequest request)
        {
            var comment = await _postsCommentsRepository.FindAsync(request.Id);
            if (comment == null)
            {
                throw new InvalidOperationException($"Comment with ID {request.Id} not found");
            }

            comment.Comment = request.Comment;
            comment.UpdatedAt = DateTime.UtcNow;

            await _postsCommentsRepository.UpdateAsync(comment);
            return new UpdatePostsCommentResponse { PostsCommentId = comment.Id };
        }

        public async Task<DeletePostsCommentResponse> DeletePostsComment(DeletePostsCommentRequest request)
        {
            var comment = await _postsCommentsRepository.FindAsync(request.Id);
            if (comment == null)
            {
                throw new InvalidOperationException($"Comment with ID {request.Id} not found");
            }

            await _postsCommentsRepository.DeleteAsync(request.Id);
            return new DeletePostsCommentResponse { Success = true };
        }

        private GetPostsCommentByIdResponse MapToResponse(PostsComments comment)
        {
            return new GetPostsCommentByIdResponse
            {
                Id = comment.Id,
                UserId = comment.UserId,
                PostsId = comment.PostsId,
                Comment = comment.Comment,
                UserName = comment.User?.Name ?? string.Empty,
                CreatedAt = comment.CreatedAt
            };
        }
    }
}