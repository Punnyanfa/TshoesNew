using Microsoft.AspNetCore.Mvc;
using FCSP.DTOs.Order; // Giả định namespace cho DTOs của Order
using FCSP.Services.OrderService; // Giả định namespace cho OrderService
using System.Threading.Tasks;

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

        /// <summary>
        /// Lấy danh sách tất cả đơn hàng
        /// </summary>
        /// <returns>Danh sách các đơn hàng</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var response = await _orderService.GetAllOrders();
            return Ok(response);
        }

        /// <summary>
        /// Lấy thông tin đơn hàng theo ID
        /// </summary>
        /// <param name="id">ID của đơn hàng</param>
        /// <returns>Thông tin chi tiết của đơn hàng</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(long id)
        {
            var request = new GetOrderByIdRequest { Id = id };
            var response = await _orderService.GetOrderById(request);
            return Ok(response);
        }

        /// <summary>
        /// Lấy danh sách đơn hàng theo ID người dùng
        /// </summary>
        /// <param name="userId">ID của người dùng</param>
        /// <returns>Danh sách đơn hàng của người dùng</returns>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetOrdersByUserId(long userId)
        {
            var request = new GetOrdersByUserIdRequest { UserId = userId };
            var response = await _orderService.GetOrdersByUserId(request);
            return Ok(response);
        }

        /// <summary>
        /// Thêm một đơn hàng mới
        /// </summary>
        /// <param name="request">Thông tin đơn hàng cần thêm</param>
        /// <returns>Thông tin đơn hàng vừa được tạo</returns>
        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] AddOrderRequest request)
        {
            var response = await _orderService.AddOrder(request);
            return CreatedAtAction(nameof(GetOrderById), new { id = response.Data.Id }, response);
        }

        /// <summary>
        /// Cập nhật thông tin đơn hàng
        /// </summary>
        /// <param name="id">ID của đơn hàng cần cập nhật</param>
        /// <param name="request">Thông tin cập nhật của đơn hàng</param>
        /// <returns>Thông tin đơn hàng sau khi cập nhật</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(long id, [FromBody] UpdateOrderRequest request)
        {
            request.Id = id;
            var response = await _orderService.UpdateOrder(request);
            return Ok(response);
        }

        /// <summary>
        /// Xóa một đơn hàng
        /// </summary>
        /// <param name="id">ID của đơn hàng cần xóa</param>
        /// <returns>Kết quả xóa đơn hàng</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(long id)
        {
            var request = new DeleteOrderRequest { Id = id };
            var response = await _orderService.DeleteOrder(request);
            return Ok(response);
        }
    }
}