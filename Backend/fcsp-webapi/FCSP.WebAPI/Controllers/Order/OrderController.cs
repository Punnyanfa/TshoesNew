using Microsoft.AspNetCore.Mvc;
using FCSP.DTOs.Order;
using FCSP.Services.OrderService;
using System.Threading.Tasks;
using FCSP.DTOs;

namespace FCSP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
            var response = await _orderService.GetAllOrders();
            return StatusCode(response.Code, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(long id)
        {
            var request = new GetOrderByIdRequest { Id = id };
            var response = await _orderService.GetOrderById(request);
            return StatusCode(response.Code, response);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetOrdersByUserId(long userId)
        {
            var request = new GetOrdersByUserIdRequest { UserId = userId };
            var response = await _orderService.GetOrdersByUserId(request);
            return StatusCode(response.Code, response);
        }

        [HttpGet("manufacturer/{manufacturerId}")]
        public async Task<IActionResult> GetOrdersByManufacturerId(long manufacturerId)
        {
            var request = new GetOrdersByManufacturerIdRequest { ManufacturerId = manufacturerId };
            var response = await _orderService.GetOrdersByManufacturerId(request);
            return StatusCode(response.Code, response);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] AddOrderRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _orderService.AddOrder(request);
            return StatusCode(response.Code, response);
        }

        [HttpPut("/status")]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] UpdateOrderStatusRequest request)
        {
            var response = await _orderService.UpdateOrderStatus(request);
            return StatusCode(response.Code, response);
        }

        [HttpPut("/shipping-status")]
        public async Task<IActionResult> UpdateOrderShippingStatus([FromBody] UpdateOrderShippingStatusRequest request)
        {
            var response = await _orderService.UpdateOrderShippingStatus(request);
            return StatusCode(response.Code, response);
        }
    }
}