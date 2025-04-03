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
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _templateService.GetAllTemplate();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTemplateById(int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var request = new GetTemplateByIdRequest { Id = id };
        var result = await _templateService.GetTemplateById(request);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddTemplate([FromBody] AddTemplateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _templateService.AddTemplate(request);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateTemplate([FromBody] UpdateTemplateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _templateService.UpdateTemplate(request);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTemplate(int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var request = new DeleteTemplateRequest { Id = id };
        var result = await _templateService.DeleteTemplate(request);
        return Ok(result);
    }

    [HttpGet("{templateId}/custom-shoe-designs")]
    public async Task<IActionResult> GetCustomShoeDesignIdsByTemplate(long templateId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var customShoeDesignIds = await _templateService.GetCustomShoeDesignIdsByTemplate(templateId);
        var response = new GetCustomShoeDesignIdsByTemplateResponse
        {
            CustomShoeDesignIds = (IEnumerable<long>)customShoeDesignIds
        };
        return Ok(response);
    }

    [HttpGet("available")]
    public async Task<IActionResult> GetAvailableTemplates()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var templates = await _templateService.GetAvailableTemplates();
        return Ok(templates);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchTemplates([FromQuery] SearchTemplatesRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _templateService.SearchTemplates(request);
        return Ok(result);
    }

    [HttpGet("popular")]
    public async Task<IActionResult> GetPopularTemplates([FromQuery] GetPopularTemplatesRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _templateService.GetPopularTemplates(request);
        return Ok(result);
    }

    [HttpPut("{id}/restore")]
    public async Task<IActionResult> RestoreTemplate(long id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var request = new RestoreTemplateRequest { Id = id };
        var result = await _templateService.RestoreTemplate(request);
        return Ok(result);
    }

    [HttpGet("{id}/stats")]
    public async Task<IActionResult> GetTemplateStats(long id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var request = new GetTemplateStatsRequest { Id = id };
        var result = await _templateService.GetTemplateStats(request);
        return Ok(result);
    }
} 