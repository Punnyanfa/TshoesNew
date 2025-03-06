using FCSP.DTOs.DesignPreview;
using FCSP.Services.DesignPreviewService;
using Microsoft.AspNetCore.Mvc;

namespace FCSP.WebAPI.Controllers.DesignPreview
{
    [ApiController]
    [Route("api/[controller]")]
    public class DesignPreviewController : ControllerBase
    {
        private readonly IDesignPreviewService _designPreviewService;

        public DesignPreviewController(IDesignPreviewService designPreviewService)
        {
            _designPreviewService = designPreviewService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDesignPreviews()
        {
            var previews = await _designPreviewService.GetAllDesignPreviews();
            return Ok(previews);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDesignPreviewById(long id)
        {
            var request = new GetDesignPreviewByIdRequest { Id = id };
            var preview = await _designPreviewService.GetDesignPreviewById(request);
            return Ok(preview);
        }

        [HttpGet("design/{designId}")]
        public async Task<IActionResult> GetPreviewsByDesign(long designId)
        {
            var request = new GetPreviewsByDesignRequest { CustomShoeDesignId = designId };
            var previews = await _designPreviewService.GetPreviewsByDesign(request);
            return Ok(previews);
        }

        [HttpPost]
        public async Task<IActionResult> AddDesignPreview([FromBody] AddDesignPreviewRequest request)
        {
            var response = await _designPreviewService.AddDesignPreview(request);
            return CreatedAtAction(nameof(GetDesignPreviewById), new { id = response.DesignPreviewId }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDesignPreview(long id, [FromBody] UpdateDesignPreviewRequest request)
        {
            if (id != request.Id)
                return BadRequest();

            var response = await _designPreviewService.UpdateDesignPreview(request);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDesignPreview(long id)
        {
            var request = new DeleteDesignPreviewRequest { Id = id };
            var response = await _designPreviewService.DeleteDesignPreview(request);
            return Ok(response);
        }
    }
} 