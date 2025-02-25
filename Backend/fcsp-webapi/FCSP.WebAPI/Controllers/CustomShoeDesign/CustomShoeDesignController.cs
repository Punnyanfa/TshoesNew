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
        var request = new GetCustomShoeDesignByIdRequest { Id = id };
        var result = await _customShoeDesignService.GetCustomShoeDesignById(request);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomShoeDesign([FromBody] AddCustomShoeDesignRequest designDto)
    {
        var result = await _customShoeDesignService.AddCustomShoeDesign(designDto);
        return CreatedAtAction(nameof(GetCustomShoeDesignById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomShoeDesign(int id, [FromBody] UpdateCustomShoeDesignRequest designDto)
    {
        designDto.Id = id;
        var result = await _customShoeDesignService.UpdateCustomShoeDesign(designDto);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomShoeDesign(int id)
    {
        var request = new DeleteCustomShoeDesignRequest { Id = id };
        var result = await _customShoeDesignService.DeleteCustomShoeDesign(request);
        if (result == null)
        {
            return NotFound();
        }
        return NoContent();
    }
} 