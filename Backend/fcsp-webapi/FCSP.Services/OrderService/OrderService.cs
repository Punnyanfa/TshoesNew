using FCSP.DTOs.Order;
using FCSP.DTOs.OrderDetail;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;

namespace FCSP.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;

        public OrderService(IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
        }

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            var response = await _orderRepository.GetAllAsync();
            return response;
        }

        public async Task<GetOrderByIdResponse> GetOrderById(GetOrderByIdRequest request)
        {
            Order order = GetEntityFromGetByIdRequest(request);
            var orderDetails = await _orderDetailRepository.GetAllAsync();
            var filteredOrderDetails = orderDetails.Where(od => od.OrderId == order.Id).ToList();
            
            return new GetOrderByIdResponse
            {
                UserId = order.UserId,
                ShippingInfoId = order.ShippingInfoId,
                TotalPrice = order.TotalPrice,
                AmountPaid = order.AmountPaid,
                Status = order.Status,
                ShippingStatus = order.ShippingStatus,
                OrderDetails = filteredOrderDetails.Select(od => new OrderDetailDto
                {
                    Id = od.Id,
                    OrderId = od.OrderId,
                    CustomShoeDesignId = od.CustomShoeDesignId,
                    Quantity = od.Quantity,
                    Price = od.Price
                }).ToList()
            };
        }

        public async Task<AddOrderResponse> AddOrder(AddOrderRequest request)
        {
            Order order = GetEntityFromAddRequest(request);
            var addedOrder = await _orderRepository.AddAsync(order);
            
            // Add order details
            foreach (var orderDetailDto in request.OrderDetails)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = addedOrder.Id,
                    CustomShoeDesignId = orderDetailDto.CustomShoeDesignId,
                    Quantity = orderDetailDto.Quantity,
                    Price = orderDetailDto.Price
                };
                await _orderDetailRepository.AddAsync(orderDetail);
            }
            
            return new AddOrderResponse { OrderId = addedOrder.Id };
        }

        public async Task<AddOrderResponse> UpdateOrder(UpdateOrderRequest request)
        {
            Order order = GetEntityFromUpdateRequest(request);
            await _orderRepository.UpdateAsync(order);
            return new AddOrderResponse { OrderId = order.Id };
        }

        public async Task<AddOrderResponse> DeleteOrder(DeleteOrderRequest request)
        {
            Order order = GetEntityFromDeleteRequest(request);
            
            // Delete related order details first
            var orderDetails = await _orderDetailRepository.GetAllAsync();
            var filteredOrderDetails = orderDetails.Where(od => od.OrderId == order.Id).ToList();
            
            foreach (var orderDetail in filteredOrderDetails)
            {
                await _orderDetailRepository.DeleteAsync(orderDetail.Id);
            }
            
            await _orderRepository.DeleteAsync(order.Id);
            return new AddOrderResponse { OrderId = order.Id };
        }

        private Order GetEntityFromGetByIdRequest(GetOrderByIdRequest request)
        {
            Order order = _orderRepository.Find(request.Id);
            if (order == null)
            {
                throw new InvalidOperationException("Order not found");
            }
            return order;
        }

        private Order GetEntityFromAddRequest(AddOrderRequest request)
        {
            return new Order
            {
                UserId = request.UserId,
                ShippingInfoId = request.ShippingInfoId,
                TotalPrice = request.TotalPrice,
                AmountPaid = request.AmountPaid,
                Status = request.Status,
                ShippingStatus = request.ShippingStatus
            };
        }

        private Order GetEntityFromUpdateRequest(UpdateOrderRequest request)
        {
            Order order = _orderRepository.Find(request.Id);
            if (order == null)
            {
                throw new InvalidOperationException("Order not found");
            }
            
            order.ShippingInfoId = request.ShippingInfoId ?? order.ShippingInfoId;
            order.TotalPrice = request.TotalPrice ?? order.TotalPrice;
            order.AmountPaid = request.AmountPaid ?? order.AmountPaid;
            order.Status = request.Status ?? order.Status;
            order.ShippingStatus = request.ShippingStatus ?? order.ShippingStatus;
            order.UpdatedAt = DateTime.Now;
            
            return order;
        }

        private Order GetEntityFromDeleteRequest(DeleteOrderRequest request)
        {
            Order order = _orderRepository.Find(request.Id);
            if (order == null)
            {
                throw new InvalidOperationException("Order not found");
            }
            return order;
        }
    }
} 