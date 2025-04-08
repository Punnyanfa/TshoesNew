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

        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] AddOrderRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _orderService.AddOrder(request);
            return StatusCode(response.Code, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(long id, [FromBody] UpdateOrderRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != request.Id)
                return BadRequest(new BaseResponseModel<object> { Code = 400, Message = "ID mismatch between route and request body" });

            var response = await _orderService.UpdateOrder(request);
            return StatusCode(response.Code, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(long id)
        {
            var request = new DeleteOrderRequest { Id = id };
            var response = await _orderService.DeleteOrder(request);
            return StatusCode(response.Code, response);
        }

        [HttpPost("process-payment")]
        public async Task<IActionResult> ProcessPayment([FromBody] ProcessPaymentRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _orderService.ProcessPayment(request);
            return StatusCode(response.Code, response);
        }

        [HttpPut("update-status")]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] UpdateOrderStatusRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _orderService.UpdateOrderStatus(request);
            return StatusCode(response.Code, response);
        }
    }
}