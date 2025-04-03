using FCSP.DTOs.CustomShoeDesignTemplate;
using FCSP.Services.TemplateService;
using Microsoft.AspNetCore.Mvc;

namespace FCSP.WebAPI.Controllers.Template;

[Route("api/[controller]")]
[ApiController]
public class TemplateController : ControllerBase
{
    private readonly ITemplateService _templateService;

    public TemplateController(ITemplateService templateService)
    {
        _templateService = templateService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTemplates()
    {
        var result = await _templateService.GetAllTemplate();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTemplateById(int id)
    {
        var request = new GetTemplateByIdRequest { Id = id };
        var result = await _templateService.GetTemplateById(request);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddTemplate([FromBody] AddTemplateRequest request)
    {
        var result = await _templateService.AddTemplate(request);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateTemplate([FromBody] UpdateTemplateRequest request)
    {
        var result = await _templateService.UpdateTemplate(request);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTemplate(int id)
    {
        var request = new DeleteTemplateRequest { Id = id };
        var result = await _templateService.DeleteTemplate(request);
        return Ok(result);
    }

    [HttpGet("{templateId}/custom-shoe-designs")]
    public async Task<IActionResult> GetCustomShoeDesignIdsByTemplate(long templateId)
    {
        var customShoeDesignIds = await _templateService.GetCustomShoeDesignIdsByTemplate(templateId);
        var response = new GetCustomShoeDesignIdsByTemplateResponse
        {
            CustomShoeDesignIds = customShoeDesignIds
        };
        return Ok(response);
    }

    [HttpGet("available")]
    public async Task<IActionResult> GetAvailableTemplates()
    {
        var templates = await _templateService.GetAvailableTemplates();
        return Ok(templates);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchTemplates([FromQuery] SearchTemplatesRequest request)
    {
        var result = await _templateService.SearchTemplates(request);
        return Ok(result);
    }

    [HttpGet("popular")]
    public async Task<IActionResult> GetPopularTemplates([FromQuery] GetPopularTemplatesRequest request)
    {
        var result = await _templateService.GetPopularTemplates(request);
        return Ok(result);
    }

    [HttpPut("{id}/restore")]
    public async Task<IActionResult> RestoreTemplate(long id)
    {
        var request = new RestoreTemplateRequest { Id = id };
        var result = await _templateService.RestoreTemplate(request);
        return Ok(result);
    }

    [HttpGet("{id}/stats")]
    public async Task<IActionResult> GetTemplateStats(long id)
    {
        var request = new GetTemplateStatsRequest { Id = id };
        var result = await _templateService.GetTemplateStats(request);
        return Ok(result);
    }
} 