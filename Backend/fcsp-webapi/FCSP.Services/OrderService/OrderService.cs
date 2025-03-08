using FCSP.DTOs.Order;
using FCSP.DTOs.OrderDetail;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FCSP.Common.Enums;

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
            Order order = await GetEntityFromGetByIdRequest(request);
            var orderDetails = await _orderDetailRepository.GetAllAsync();
            var filteredOrderDetails = orderDetails.Where(od => od.OrderId == order.Id).ToList();
            
            return new GetOrderByIdResponse
            {
                Id = order.Id,
                UserId = order.UserId,
                ShippingInfoId = order.ShippingInfoId,
                VoucherId = order.VoucherId,
                TotalPrice = order.TotalPrice,
                Status = order.Status,
                Note = null, 
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt,
                OrderDetails = filteredOrderDetails.Select(od => new OrderDetailDto
                {
                    CustomShoeDesignId = od.CustomShoeDesignId,
                    Quantity = od.Quantity,
                    UnitPrice = od.Price,
                    Size = null 
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
                    Price = orderDetailDto.UnitPrice,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                await _orderDetailRepository.AddAsync(orderDetail);
            }
            
            return new AddOrderResponse 
            { 
                Id = addedOrder.Id,
                TotalPrice = addedOrder.TotalPrice,
                Status = (int)addedOrder.Status
            };
        }

        public async Task<UpdateOrderResponse> UpdateOrder(UpdateOrderRequest request)
        {
            Order order = await GetEntityFromUpdateRequest(request);
            await _orderRepository.UpdateAsync(order);
            return new UpdateOrderResponse 
            { 
                Id = order.Id,
                TotalPrice = order.TotalPrice,
                Status = order.Status
            };
        }

        public async Task<DeleteOrderResponse> DeleteOrder(DeleteOrderRequest request)
        {
            Order order = await GetEntityFromDeleteRequest(request);
            
            // Delete related order details first
            var orderDetails = await _orderDetailRepository.GetAllAsync();
            var filteredOrderDetails = orderDetails.Where(od => od.OrderId == order.Id).ToList();
            
            foreach (var orderDetail in filteredOrderDetails)
            {
                await _orderDetailRepository.DeleteAsync(orderDetail.Id);
            }
            
            await _orderRepository.DeleteAsync(order.Id);
            return new DeleteOrderResponse { Success = true };
        }

        private async Task<Order> GetEntityFromGetByIdRequest(GetOrderByIdRequest request)
        {
            Order order = await _orderRepository.FindAsync(request.Id);
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
                VoucherId = request.VoucherId,
                TotalPrice = request.TotalPrice,
                AmountPaid = 0, // Set appropriate default or get from request
                Status = OrderStatus.Pending,
                ShippingStatus = OrderShippingStatus.Preparing,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }

        private async Task<Order> GetEntityFromUpdateRequest(UpdateOrderRequest request)
        {
            Order order = await _orderRepository.FindAsync(request.Id);
            if (order == null)
            {
                throw new InvalidOperationException("Order not found");
            }
            
            order.ShippingInfoId = request.ShippingInfoId;
            order.VoucherId = request.VoucherId;
            order.TotalPrice = request.TotalPrice;
            order.Status = request.Status;
            order.UpdatedAt = DateTime.UtcNow;
            
            return order;
        }

        private async Task<Order> GetEntityFromDeleteRequest(DeleteOrderRequest request)
        {
            Order order = await _orderRepository.FindAsync(request.Id);
            if (order == null)
            {
                throw new InvalidOperationException("Order not found");
            }
            return order;
        }
    }
} 