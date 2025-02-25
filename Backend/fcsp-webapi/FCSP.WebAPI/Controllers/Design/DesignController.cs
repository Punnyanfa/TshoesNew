using FCSP.DTOs.Design;
using FCSP.Services.DesignService;
using Microsoft.AspNetCore.Mvc;

namespace FCSP.WebAPI.Controllers.Design;

[Route("api/[controller]")]
[ApiController]
public class DesignController : ControllerBase
{
    private readonly IDesignService _designService;

    public DesignController(IDesignService designService)
    {
        _designService = designService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDesigns()
    {
        var result = await _designService.GetAllTemplate();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDesignById(int id)
    {
        var request = new GetDesignByIdRequest { Id = id };
        var result = await _designService.GetTemplateById(request);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddDesign([FromBody] AddDesignRequest request)
    {
        var result = await _designService.AddDesign(request);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateDesign([FromBody] UpdateDesignRequest request)
    {
        var result = await _designService.UpdateDesign(request);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDesign(int id)
    {
        var request = new DeleteDesignRequest { Id = id };
        var result = await _designService.DeleteDesign(request);
        return Ok(result);
    }
} 