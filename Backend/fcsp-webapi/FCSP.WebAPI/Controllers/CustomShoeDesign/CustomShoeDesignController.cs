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
    public async Task<IActionResult> GetAllPublicDesigns()
    {
        var response = await _customShoeDesignService.GetAllPublicDesigns();
        return StatusCode(response.Code, response);
    }
    
    /// <summary>
    /// Get top 5 best selling designs
    /// </summary>
    /// <returns>List of top 5 best selling designs</returns>
    [HttpGet("top-5-best-selling")]
    public async Task<IActionResult> GetTopFiveBestSellingDesigns()
    {
        var response = await _customShoeDesignService.GetTopFiveBestSellingPublicDesigns();
        return StatusCode(response.Code, response);
    }

    /// <summary>
    /// Get a custom shoe design by ID
    /// </summary>
    /// <param name="request">Custom shoe design ID</param>
    /// <returns>Custom shoe design details</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomShoeDesignById([FromRoute]long id)
    {
        GetCustomShoeDesignByIdRequest request = new GetCustomShoeDesignByIdRequest { Id = id };
        var response = await _customShoeDesignService.GetDesignById(request);
        return StatusCode(response.Code, response);
    }

    /// <summary>
    /// Get all designs by a specific user
    /// </summary>
    /// <param name="request">User ID</param>
    /// <returns>List of designs by user</returns>
    [HttpGet("user/{id}")]
    public async Task<IActionResult> GetDesignsByUser([FromRoute]long id)
    {
        GetCustomShoeDesignsByUserIdRequest request = new GetCustomShoeDesignsByUserIdRequest { UserId = id };
        var response = await _customShoeDesignService.GetDesignsByUserId(request);
        return StatusCode(response.Code, response);
    }

    /// <summary>
    /// Create a new custom shoe design
    /// </summary>
    /// <param name="request">Custom shoe design data</param>
    /// <returns>Created custom shoe design</returns>
    [HttpPost]
    public async Task<IActionResult> CreateCustomShoeDesign([FromBody] AddCustomShoeDesignRequest request)
    {
        var response = await _customShoeDesignService.AddCustomShoeDesign(request);
        return StatusCode(response.Code, response);
    }


    /// <summary>
    /// Update the status of a custom shoe design
    /// </summary>
    /// <param name="request">Custom shoe design ID and status</param>
    /// <returns>Updated custom shoe design status</returns>
    [HttpPut("status")]
    public async Task<IActionResult> UpdateCustomShoeDesignStatus([FromBody] UpdateCustomShoeDesignStatusRequest request)
    {
        var response = await _customShoeDesignService.UpdateCustomShoeDesignStatus(request);
        return StatusCode(response.Code, response);
    }

    /// <summary>
    /// Update an existing custom shoe design
    /// </summary>
    /// <param name="request">Custom shoe design ID</param>
    /// <param name="designDto">Updated custom shoe design data</param>
    /// <returns>Updated custom shoe design</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomShoeDesign([FromRoute]long id, [FromBody] UpdateCustomShoeDesignRequest request)
    {
        var response = await _customShoeDesignService.UpdateCustomShoeDesign(request);
        return StatusCode(response.Code, response);
    }

    /// <summary>
    /// Delete a custom shoe design
    /// </summary>
    /// <param name="request">Custom shoe design ID</param>
    /// <returns>No content if successful</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomShoeDesign([FromRoute]long id)
    {
        DeleteCustomShoeDesignRequest request = new DeleteCustomShoeDesignRequest { Id = id };
        var response = await _customShoeDesignService.DeleteCustomShoeDesign(request);
        return StatusCode(response.Code, response);
    }
} 