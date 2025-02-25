using FCSP.DTOs.PostComment;
using FCSP.Models.Entities;
using FCSP.Repositories.PostComment;

namespace FCSP.Services.PostCommentService
{
    public class PostCommentService : IPostCommentService
    {
        private readonly IPostCommentRepository _postCommentRepository;

        public PostCommentService(IPostCommentRepository postCommentRepository)
        {
            _postCommentRepository = postCommentRepository;
        }

        public async Task<IEnumerable<GetPostCommentByIdResponse>> GetAllPostComments()
        {
            var comments = await _postCommentRepository.GetAllAsync();
            return comments.Select(MapToDetailedResponse);
        }

        public async Task<GetPostCommentByIdResponse> GetPostCommentById(GetPostCommentByIdRequest request)
        {
            PostComment comment = GetEntityFromGetByIdRequest(request);
            return MapToDetailedResponse(comment);
        }

        public async Task<IEnumerable<GetPostCommentByIdResponse>> GetPostCommentsByPost(GetPostCommentsByPostRequest request)
        {
            var comments = await _postCommentRepository.GetCommentsByPostIdAsync(request.PostId);
            return comments.Select(MapToDetailedResponse);
        }

        public async Task<IEnumerable<GetPostCommentByIdResponse>> GetPostCommentsByUser(GetPostCommentsByUserRequest request)
        {
            var comments = await _postCommentRepository.GetCommentsByUserIdAsync(request.UserId);
            return comments.Select(MapToDetailedResponse);
        }

        public async Task<AddPostCommentResponse> AddPostComment(AddPostCommentRequest request)
        {
            PostComment comment = GetEntityFromAddRequest(request);
            var addedComment = await _postCommentRepository.AddAsync(comment);
            
            return new AddPostCommentResponse
            {
                Id = addedComment.Id,
                UserId = addedComment.UserId,
                PostId = addedComment.PostId,
                Comment = addedComment.Comment,
                CreatedAt = addedComment.CreatedAt
            };
        }

        public async Task<GetPostCommentByIdResponse> UpdatePostComment(UpdatePostCommentRequest request)
        {
            PostComment comment = GetEntityFromUpdateRequest(request);
            await _postCommentRepository.UpdateAsync(comment);
            return MapToDetailedResponse(comment);
        }

        public async Task<GetPostCommentByIdResponse> DeletePostComment(DeletePostCommentRequest request)
        {
            PostComment comment = GetEntityFromDeleteRequest(request);
            var response = MapToDetailedResponse(comment);
            await _postCommentRepository.DeleteAsync(comment.Id);
            return response;
        }

        private GetPostCommentByIdResponse MapToDetailedResponse(PostComment comment)
        {
            return new GetPostCommentByIdResponse
            {
                Id = comment.Id,
                UserId = comment.UserId,
                PostId = comment.PostId,
                Comment = comment.Comment,
                UserName = comment.User?.Name,
                PostTitle = comment.Post?.Title,
                CreatedAt = comment.CreatedAt,
                UpdatedAt = comment.UpdatedAt
            };
        }

        private PostComment GetEntityFromGetByIdRequest(GetPostCommentByIdRequest request)
        {
            PostComment comment = _postCommentRepository.Find(request.Id);
            if (comment == null)
            {
                throw new InvalidOperationException("PostComment not found");
            }
            return comment;
        }

        private PostComment GetEntityFromAddRequest(AddPostCommentRequest request)
        {
            return new PostComment
            {
                UserId = request.UserId,
                PostId = request.PostId,
                Comment = request.Comment
            };
        }

        private PostComment GetEntityFromUpdateRequest(UpdatePostCommentRequest request)
        {
            PostComment comment = _postCommentRepository.Find(request.Id);
            if (comment == null)
            {
                throw new InvalidOperationException("PostComment not found");
            }
            
            comment.Comment = request.Comment ?? comment.Comment;
            comment.UpdatedAt = DateTime.Now;
            
            return comment;
        }

        private PostComment GetEntityFromDeleteRequest(DeletePostCommentRequest request)
        {
            PostComment comment = _postCommentRepository.Find(request.Id);
            if (comment == null)
            {
                throw new InvalidOperationException("PostComment not found");
            }
            return comment;
        }
    }
} 