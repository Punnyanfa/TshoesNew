using FCSP.DTOs.Texture;
using FCSP.Services.TextureService;
using Microsoft.AspNetCore.Mvc;

namespace FCSP.WebAPI.Controllers.Texture;

[Route("api/[controller]")]
[ApiController]
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
        var result = await _textureService.GetAllTexture();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTextureById(int id)
    {
        var request = new GetTextureByIdRequest { Id = id };
        var result = await _textureService.GetTextureById(request);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddTexture([FromBody] AddTextureRequest request)
    {
        var result = await _textureService.AddTexture(request);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateTexture([FromBody] UpdateTextureRequest request)
    {
        var result = await _textureService.UpdateTexture(request);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTexture(int id)
    {
        var request = new DeleteTextureRequest { Id = id };
        var result = await _textureService.DeleteTexture(request);
        return Ok(result);
    }
} 