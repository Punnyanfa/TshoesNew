using FCSP.DTOs.CustomShoeDesign;
using FCSP.Services.CustomShoeDesignService;
using Microsoft.AspNetCore.Mvc;

namespace FCSP.WebAPI.Controllers.CustomShoeDesign;

[Route("api/[controller]")]
[ApiController]
public class CustomShoeDesignController : ControllerBase
{
    private readonly ICustomShoeDesignService _customShoeDesignService;

    public CustomShoeDesignController(ICustomShoeDesignService customShoeDesignService)
    {
        _customShoeDesignService = customShoeDesignService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCustomShoeDesigns()
    {
        var result = await _customShoeDesignService.GetAllCustomShoeDesigns();
        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomShoeDesignById(int id)
    {
        var result = await _customShoeDesignService.GetCustomShoeDesignById(id);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomShoeDesign([FromBody] CustomShoeDesignCreateDto designDto)
    {
        var result = await _customShoeDesignService.CreateCustomShoeDesign(designDto);
        return CreatedAtAction(nameof(GetCustomShoeDesignById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomShoeDesign(int id, [FromBody] CustomShoeDesignUpdateDto designDto)
    {
        var result = await _customShoeDesignService.UpdateCustomShoeDesign(id, designDto);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomShoeDesign(int id)
    {
        var result = await _customShoeDesignService.DeleteCustomShoeDesign(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
} 