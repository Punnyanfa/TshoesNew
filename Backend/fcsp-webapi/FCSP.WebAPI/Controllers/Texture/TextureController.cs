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
        public async Task<IActionResult> GetTexturesByUser(GetTexturesByUserIdRequest request)
        {
            var response = await _textureService.GetTexturesByUserId(request);
            return StatusCode(response.Code, response);
        }

        [HttpPost]
        public async Task<IActionResult> AddTexture([FromForm] AddTextureRequest request)
        {
            var response = await _textureService.AddTexture(request);
            return StatusCode(response.Code, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTexture(DeleteTextureRequest request)
        {
            var response = await _textureService.DeleteTexture(request);
            return StatusCode(response.Code, response);
        }
    }
} 