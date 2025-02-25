using FCSP.DTOs.CustomShoeDesignTexture;
using FCSP.Services.CustomShoeDesignTextureService;
using Microsoft.AspNetCore.Mvc;

namespace FCSP.WebAPI.Controllers.CustomShoeDesignTexture;

[Route("api/[controller]")]
[ApiController]
public class CustomShoeDesignTextureController : ControllerBase
{
    private readonly ICustomShoeDesignTextureService _customShoeDesignTextureService;

    public CustomShoeDesignTextureController(ICustomShoeDesignTextureService customShoeDesignTextureService)
    {
        _customShoeDesignTextureService = customShoeDesignTextureService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCustomShoeDesignTextures()
    {
        var result = await _customShoeDesignTextureService.GetAllCustomShoeDesignTextures();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomShoeDesignTextureById(int id)
    {
        var request = new GetCustomShoeDesignTextureByIdRequest { Id = id };
        var result = await _customShoeDesignTextureService.GetCustomShoeDesignTextureById(request);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddCustomShoeDesignTexture([FromBody] AddCustomShoeDesignTextureRequest request)
    {
        var result = await _customShoeDesignTextureService.AddCustomShoeDesignTexture(request);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCustomShoeDesignTexture([FromBody] UpdateCustomShoeDesignTextureRequest request)
    {
        var result = await _customShoeDesignTextureService.UpdateCustomShoeDesignTexture(request);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomShoeDesignTexture(int id)
    {
        var request = new DeleteCustomShoeDesignTextureRequest { Id = id };
        var result = await _customShoeDesignTextureService.DeleteCustomShoeDesignTexture(request);
        return Ok(result);
    }
} 