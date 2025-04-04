using FCSP.DTOs.Size;
using FCSP.Services.SizeService;
using Microsoft.AspNetCore.Mvc;

namespace FCSP.WebAPI.Controllers.Size;

[Route("api/[controller]")]
[ApiController]
public class SizeController : ControllerBase
{
    private readonly ISizeService _sizeService;

    public SizeController(ISizeService sizeService)
    {
        _sizeService = sizeService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllSizes()
    {
        var result = await _sizeService.GetAllSizesAsync();
        return StatusCode(result.Code, result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSizeById(long id)
    {
        var request = new GetSizeByIdRequest { Id = id };
        var result = await _sizeService.GetSizeByIdAsync(request);
        return StatusCode(result.Code, result);
    }

    [HttpPost]
    public async Task<IActionResult> AddSize([FromBody] AddSizeRequest request)
    {
        var result = await _sizeService.CreateSizeAsync(request);
        return StatusCode(result.Code, result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateSize([FromBody] UpdateSizeRequest request)
    {
        var result = await _sizeService.UpdateSizeAsync(request);
        return StatusCode(result.Code, result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSize(long id)
    {
        var request = new DeleteSizeRequest { Id = id };
        var result = await _sizeService.DeleteSizeAsync(request);
        return StatusCode(result.Code, result);
    }
} 