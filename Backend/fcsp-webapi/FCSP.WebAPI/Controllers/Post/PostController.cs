using FCSP.DTOs.Post;
using FCSP.Services.PostService;
using Microsoft.AspNetCore.Mvc;

namespace FCSP.WebAPI.Controllers.Post
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        {
            var result = await _postService.GetAllPosts();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(int id)
        {
            var request = new GetPostByIdRequest { Id = id };
            var result = await _postService.GetPostById(request);
            return Ok(result);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetPostsByUser(int userId)
        {
            var request = new GetPostsByUserRequest { UserId = userId };
            var result = await _postService.GetPostsByUser(request);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddPost([FromBody] AddPostRequest request)
        {
            var result = await _postService.AddPost(request);
            return CreatedAtAction(nameof(GetPostById), new { id = result.Id }, result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePost([FromBody] UpdatePostRequest request)
        {
            var result = await _postService.UpdatePost(request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var request = new DeletePostRequest { Id = id };
            var result = await _postService.DeletePost(request);
            return Ok(result);
        }
    }
} 