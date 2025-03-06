using FCSP.DTOs.PostsComments;
using FCSP.Services.PostsCommentsService;
using Microsoft.AspNetCore.Mvc;

namespace FCSP.WebAPI.Controllers.PostsComments
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsCommentsController : ControllerBase
    {
        private readonly IPostsCommentsService _postsCommentsService;

        public PostsCommentsController(IPostsCommentsService postsCommentsService)
        {
            _postsCommentsService = postsCommentsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPostsComments()
        {
            var comments = await _postsCommentsService.GetAllPostsComments();
            return Ok(comments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostsCommentById(long id)
        {
            var request = new GetPostsCommentByIdRequest { Id = id };
            var comment = await _postsCommentsService.GetPostsCommentById(request);
            return Ok(comment);
        }

        [HttpGet("post/{postId}")]
        public async Task<IActionResult> GetCommentsByPost(long postId)
        {
            var request = new GetCommentsByPostRequest { PostId = postId };
            var comments = await _postsCommentsService.GetCommentsByPost(request);
            return Ok(comments);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetCommentsByUser(long userId)
        {
            var request = new GetCommentsByUserRequest { UserId = userId };
            var comments = await _postsCommentsService.GetCommentsByUser(request);
            return Ok(comments);
        }

        [HttpPost]
        public async Task<IActionResult> AddPostsComment([FromBody] AddPostsCommentRequest request)
        {
            var response = await _postsCommentsService.AddPostsComment(request);
            return CreatedAtAction(nameof(GetPostsCommentById), new { id = response.PostsCommentId }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePostsComment(long id, [FromBody] UpdatePostsCommentRequest request)
        {
            if (id != request.Id)
                return BadRequest();

            var response = await _postsCommentsService.UpdatePostsComment(request);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePostsComment(long id)
        {
            var request = new DeletePostsCommentRequest { Id = id };
            var response = await _postsCommentsService.DeletePostsComment(request);
            return Ok(response);
        }
    }
} 