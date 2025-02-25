using FCSP.DTOs.OrderDetail;
using FCSP.Services.OrderDetailService;
using Microsoft.AspNetCore.Mvc;

namespace FCSP.WebAPI.Controllers.OrderDetail;

[Route("api/[controller]")]
[ApiController]
public class OrderDetailController : ControllerBase
{
    private readonly IOrderDetailService _orderDetailService;

    public OrderDetailController(IOrderDetailService orderDetailService)
    {
        _orderDetailService = orderDetailService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllOrderDetails()
    {
        var result = await _orderDetailService.GetAllOrderDetails();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderDetailById(int id)
    {
        var request = new GetOrderDetailByIdRequest { Id = id };
        var result = await _orderDetailService.GetOrderDetailById(request);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddOrderDetail([FromBody] AddOrderDetailRequest request)
    {
        var result = await _orderDetailService.AddOrderDetail(request);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateOrderDetail([FromBody] UpdateOrderDetailRequest request)
    {
        var result = await _orderDetailService.UpdateOrderDetail(request);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrderDetail(int id)
    {
        var request = new DeleteOrderDetailRequest { Id = id };
        var result = await _orderDetailService.DeleteOrderDetail(request);
        return Ok(result);
    }
} 