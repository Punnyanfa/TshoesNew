using FCSP.DTOs.CustomShoeDesign;
using FCSP.Services.CustomShoeDesignService;
using Microsoft.AspNetCore.Mvc;

namespace FCSP.WebAPI.Controllers.CustomShoeDesign;

/// <summary>
/// API Controller for managing custom shoe designs
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class CustomShoeDesignController : ControllerBase
{
    private readonly ICustomShoeDesignService _customShoeDesignService;

    public CustomShoeDesignController(ICustomShoeDesignService customShoeDesignService)
    {
        _customShoeDesignService = customShoeDesignService;
    }

    /// <summary>
    /// Get all custom shoe designs
    /// </summary>
    /// <returns>List of all custom shoe designs</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllCustomShoeDesigns()
    {
        var result = await _customShoeDesignService.GetAllCustomShoeDesigns();
        return Ok(result);
    }
    
    /// <summary>
    /// Get a custom shoe design by ID
    /// </summary>
    /// <param name="id">Custom shoe design ID</param>
    /// <returns>Custom shoe design details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCustomShoeDesignById(long id)
    {
        try
        {
            var request = new GetCustomShoeDesignByIdRequest { Id = id };
            var result = await _customShoeDesignService.GetCustomShoeDesignById(request);
            if (result == null)
            {
                return NotFound(new { message = $"Custom shoe design with ID {id} not found" });
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Create a new custom shoe design
    /// </summary>
    /// <param name="designDto">Custom shoe design data</param>
    /// <returns>Created custom shoe design</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCustomShoeDesign([FromBody] AddCustomShoeDesignRequest designDto)
    {
        try
        {
            var result = await _customShoeDesignService.AddCustomShoeDesign(designDto);
            return CreatedAtAction(nameof(GetCustomShoeDesignById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Update an existing custom shoe design
    /// </summary>
    /// <param name="id">Custom shoe design ID</param>
    /// <param name="designDto">Updated custom shoe design data</param>
    /// <returns>Updated custom shoe design</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateCustomShoeDesign(long id, [FromBody] UpdateCustomShoeDesignRequest designDto)
    {
        try
        {
            designDto.Id = id;
            var result = await _customShoeDesignService.UpdateCustomShoeDesign(designDto);
            if (result == null)
            {
                return NotFound(new { message = $"Custom shoe design with ID {id} not found" });
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Delete a custom shoe design
    /// </summary>
    /// <param name="id">Custom shoe design ID</param>
    /// <returns>No content if successful</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCustomShoeDesign(long id)
    {
        try
        {
            var request = new DeleteCustomShoeDesignRequest { Id = id };
            var result = await _customShoeDesignService.DeleteCustomShoeDesign(request);
            if (result == null || !result.Success)
            {
                return NotFound(new { message = $"Custom shoe design with ID {id} not found" });
            }
            return NoContent();
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
    
    /// <summary>
    /// Get all designs by a specific user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <returns>List of designs by user</returns>
    [HttpGet("user/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDesignsByUser(long userId)
    {
        try
        {
            var request = new GetDesignsByUserRequest { UserId = userId };
            var result = await _customShoeDesignService.GetDesignsByUser(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
} 