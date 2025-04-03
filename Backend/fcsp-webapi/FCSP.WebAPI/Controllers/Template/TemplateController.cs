using FCSP.DTOs;
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
        return StatusCode(result.Code, result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTemplateById(int id)
    {
        var request = new GetTemplateByIdRequest { Id = id };
        var result = await _templateService.GetTemplateById(request);
        return StatusCode(result.Code, result);
    }

    [HttpPost]
    public async Task<IActionResult> AddTemplate([FromBody] AddTemplateRequest request)
    {
        var result = await _templateService.AddTemplate(request);
        return StatusCode(result.Code, result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateTemplate([FromBody] UpdateTemplateRequest request)
    {
        var result = await _templateService.UpdateTemplate(request);
        return StatusCode(result.Code, result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTemplate(int id)
    {
        var request = new DeleteTemplateRequest { Id = id };
        var result = await _templateService.DeleteTemplate(request);
        return StatusCode(result.Code, result);
    }
} 