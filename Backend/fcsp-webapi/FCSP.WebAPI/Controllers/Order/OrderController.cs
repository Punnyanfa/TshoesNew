using FCSP.DTOs.Order;
using FCSP.Services.OrderService;
using Microsoft.AspNetCore.Mvc;

namespace FCSP.WebAPI.Controllers.Order;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllOrders()
    {
        var result = await _orderService.GetAllOrders();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(int id)
    {
        var request = new GetOrderByIdRequest { Id = id };
        var result = await _orderService.GetOrderById(request);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddOrder([FromBody] AddOrderRequest request)
    {
        var result = await _orderService.AddOrder(request);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderRequest request)
    {
        var result = await _orderService.UpdateOrder(request);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var request = new DeleteOrderRequest { Id = id };
        var result = await _orderService.DeleteOrder(request);
        return Ok(result);
    }
} 