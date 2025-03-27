using FCSP.DTOs.Order;
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
        return StatusCode(result.Code, result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetShippingInfoById(long id)
    {
        var request = new GetShippingInfoByIdRequest { Id = id };
        var result = await _shippingInfoService.GetShippingInfoById(request);
        return StatusCode(result.Code, result);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetShippingInfosByUserId(long userId)
    {
        var request = new GetShippingInfosByUserRequest { UserId = userId };
        var result = await _shippingInfoService.GetShippingInfosByUserId(request);
        return StatusCode(result.Code, result);
    }

    [HttpPost]
    public async Task<IActionResult> AddShippingInfo([FromBody] AddShippingInfoRequest request)
    {
        var result = await _shippingInfoService.AddShippingInfo(request);
        return StatusCode(result.Code, result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateShippingInfo([FromBody] UpdateShippingInfoRequest request)
    {
        var result = await _shippingInfoService.UpdateShippingInfo(request);
        return StatusCode(result.Code, result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteShippingInfo(long id)
    {
        var request = new DeleteShippingInfoRequest { Id = id };
        var result = await _shippingInfoService.DeleteShippingInfo(request);
        return StatusCode(result.Code, result);
    }

    [HttpPost("set-default")]
    public async Task<IActionResult> SetDefaultShippingInfo([FromBody] SetDefaultShippingInfoRequest request)
    {
        var result = await _shippingInfoService.SetDefaultShippingInfo(request);
        return StatusCode(result.Code, result);
    }
}