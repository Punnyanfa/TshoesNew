using FCSP.DTOs.PostComment;
using FCSP.Services.PostCommentService;
using Microsoft.AspNetCore.Mvc;

namespace FCSP.WebAPI.Controllers.PostComment
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostCommentController : ControllerBase
    {
        private readonly IPostCommentService _postCommentService;

        public PostCommentController(IPostCommentService postCommentService)
        {
            _postCommentService = postCommentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPostComments()
        {
            var result = await _postCommentService.GetAllPostComments();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostCommentById(int id)
        {
            var request = new GetPostCommentByIdRequest { Id = id };
            var result = await _postCommentService.GetPostCommentById(request);
            return Ok(result);
        }

        [HttpGet("post/{postId}")]
        public async Task<IActionResult> GetPostCommentsByPost(int postId)
        {
            var request = new GetPostCommentsByPostRequest { PostId = postId };
            var result = await _postCommentService.GetPostCommentsByPost(request);
            return Ok(result);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetPostCommentsByUser(int userId)
        {
            var request = new GetPostCommentsByUserRequest { UserId = userId };
            var result = await _postCommentService.GetPostCommentsByUser(request);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddPostComment([FromBody] AddPostCommentRequest request)
        {
            var result = await _postCommentService.AddPostComment(request);
            return CreatedAtAction(nameof(GetPostCommentById), new { id = result.Id }, result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePostComment([FromBody] UpdatePostCommentRequest request)
        {
            var result = await _postCommentService.UpdatePostComment(request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePostComment(int id)
        {
            var request = new DeletePostCommentRequest { Id = id };
            var result = await _postCommentService.DeletePostComment(request);
            return Ok(result);
        }
    }
} 