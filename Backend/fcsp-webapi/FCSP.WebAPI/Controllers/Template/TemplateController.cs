using FCSP.DTOs.CustomShoeDesignTemplate;
using FCSP.Services.TemplateService;
using Microsoft.AspNetCore.Mvc;

namespace FCSP.WebAPI.Controllers.Template
{
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
        public async Task<IActionResult> GetTemplateById(long id) // Changed to long
        {
            var request = new GetTemplateByIdRequest { Id = id };
            var result = await _templateService.GetTemplateById(request);
            return StatusCode(result.Code, result);
        }

        [HttpPost]
        public async Task<IActionResult> AddTemplate([FromBody] AddTemplateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _templateService.AddTemplate(request);
            return StatusCode(result.Code, result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTemplate([FromBody] UpdateTemplateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _templateService.UpdateTemplate(request);
            return StatusCode(result.Code, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTemplate(long id) // Changed to long
        {
            var request = new DeleteTemplateRequest { Id = id };
            var result = await _templateService.DeleteTemplate(request);
            return StatusCode(result.Code, result);
        }

        [HttpGet("{templateId}/custom-shoe-designs")]
        public async Task<IActionResult> GetCustomShoeDesignIdsByTemplate(long templateId)
        {
            var result = await _templateService.GetCustomShoeDesignIdsByTemplate(templateId);
            return StatusCode(result.Code, result); // Fixed response handling
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableTemplates()
        {
            var result = await _templateService.GetAvailableTemplates();
            return StatusCode(result.Code, result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchTemplates([FromQuery] SearchTemplatesRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _templateService.SearchTemplates(request);
            return StatusCode(result.Code, result);
        }

        [HttpGet("popular")]
        public async Task<IActionResult> GetPopularTemplates([FromQuery] GetPopularTemplatesRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _templateService.GetPopularTemplates(request);
            return StatusCode(result.Code, result);
        }

        [HttpPut("{id}/restore")]
        public async Task<IActionResult> RestoreTemplate(long id)
        {
            var request = new RestoreTemplateRequest { Id = id };
            var result = await _templateService.RestoreTemplate(request);
            return StatusCode(result.Code, result);
        }

        [HttpGet("{id}/stats")]
        public async Task<IActionResult> GetTemplateStats(long id)
        {
            var request = new GetTemplateStatsRequest { Id = id };
            var result = await _templateService.GetTemplateStats(request);
            return StatusCode(result.Code, result);
        }
    }
}