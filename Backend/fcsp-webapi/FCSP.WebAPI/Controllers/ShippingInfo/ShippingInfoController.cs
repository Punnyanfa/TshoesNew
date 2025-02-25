using FCSP.DTOs.ShippingInfo;
using FCSP.Services.ShippingInfoService;
using Microsoft.AspNetCore.Mvc;

namespace FCSP.WebAPI.Controllers.ShippingInfo;

[Route("api/[controller]")]
[ApiController]
public class ShippingInfoController : ControllerBase
{
    private readonly IShippingInfoService _shippingInfoService;

    public ShippingInfoController(IShippingInfoService shippingInfoService)
    {
        _shippingInfoService = shippingInfoService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllShippingInfos()
    {
        var result = await _shippingInfoService.GetAllShippingInfo();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetShippingInfoById(int id)
    {
        var request = new GetShippingInfoByIdRequest { Id = id };
        var result = await _shippingInfoService.GetShippingInfoById(request);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddShippingInfo([FromBody] AddShippingInfoRequest request)
    {
        var result = await _shippingInfoService.AddShippingInfo(request);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateShippingInfo([FromBody] UpdateShippingInfoRequest request)
    {
        var result = await _shippingInfoService.UpdateShippingInfo(request);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteShippingInfo(int id)
    {
        var request = new DeleteShippingInfoRequest { Id = id };
        var result = await _shippingInfoService.DeleteShippingInfo(request);
        return Ok(result);
    }
} 