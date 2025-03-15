using FCSP.DTOs.Texture;
using FCSP.Services.TextureService;
using Microsoft.AspNetCore.Mvc;

namespace FCSP.WebAPI.Controllers.Texture
{
    [ApiController]
    [Route("api/[controller]")]
    public class TextureController : ControllerBase
    {
        private readonly ITextureService _textureService;

        public TextureController(ITextureService textureService)
        {
            _textureService = textureService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTextures()
        {
            var response = await _textureService.GetAllTextures();
            return StatusCode(response.Code, response);
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableTextures()
        {
            var response = await _textureService.GetAvailableTextures();
            return StatusCode(response.Code, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTextureById(GetTextureByIdRequest request)
        {
            var response = await _textureService.GetTextureById(request);
            return StatusCode(response.Code, response);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetTexturesByUser(GetTexturesByUserRequest request)
        {
            var response = await _textureService.GetTexturesByUser(request);
            return StatusCode(response.Code, response);
        }

        //[HttpPost("generate")]
        //public async Task<IActionResult> GenerateImage([FromBody] GenerateImageRequest request)
        //{
        //    if (string.IsNullOrEmpty(request.Prompt))
        //    {
        //    return BadRequest("Prompt is required");
        //    }

        //    var result = await _textureService.GenerateAndSaveImage(request);

        //    if (!result.Success)
        //    {
        //        return StatusCode(500, result.ErrorMessage);
        //    }

        //    return Ok(result);
        //}

        [HttpPost]
        public async Task<IActionResult> AddTexture([FromBody] AddTextureRequest request)
        {
            var response = await _textureService.AddTexture(request);
            return StatusCode(response.Code, response);
        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateTexture(long id, [FromBody] UpdateTextureRequest request)
        //{
        //    if (id != request.Id)
        //        return BadRequest();

        //    var response = await _textureService.UpdateTexture(request);
        //    return Ok(response);
        //}

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTexture(long id)
        {
            var request = new DeleteTextureRequest { Id = id };
            var response = await _textureService.DeleteTexture(request);
            return StatusCode(response.Code, response);
        }
    }
} 