using FCSP.Common.Enums;
using FCSP.DTOs;
using FCSP.DTOs.Order;
using FCSP.DTOs.OrderDetail;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        #region Public Methods

        public async Task<BaseResponseModel<List<GetOrderByUserIdResponse>>> GetOrdersByUserId(GetOrdersByUserIdRequest request)
        {
            try
            {
                var orders = await GetOrdersByUserIdAsync(request);
                return new BaseResponseModel<List<GetOrderByUserIdResponse>>
                {
                    Code = 200,
                    Message = "Orders retrieved successfully",
                    Data = orders
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<List<GetOrderByUserIdResponse>>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<GetOrderByIdResponse>> GetOrderById(GetOrderByIdRequest request)
        {
            try
            {
                var order = await GetOrderByIdAsync(request);
                return new BaseResponseModel<GetOrderByIdResponse>
                {
                    Code = 200,
                    Message = "Order retrieved successfully",
                    Data = order
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<GetOrderByIdResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<List<GetOrderByIdResponse>>> GetAllOrders()
        {
            try
            {
                var orders = await GetAllOrdersAsync();
                return new BaseResponseModel<List<GetOrderByIdResponse>>
                {
                    Code = 200,
                    Message = "All orders retrieved successfully",
                    Data = orders
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<List<GetOrderByIdResponse>>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<AddOrderResponse>> AddOrder(AddOrderRequest request)
        {
            try
            {
                var addedOrder = await AddOrderAsync(request);
                return new BaseResponseModel<AddOrderResponse>
                {
                    Code = 201,
                    Message = "Order added successfully",
                    Data = addedOrder
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<AddOrderResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<UpdateOrderResponse>> UpdateOrder(UpdateOrderRequest request)
        {
            try
            {
                var updatedOrder = await UpdateOrderAsync(request);
                return new BaseResponseModel<UpdateOrderResponse>
                {
                    Code = 200,
                    Message = "Order updated successfully",
                    Data = updatedOrder
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<UpdateOrderResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<DeleteOrderResponse>> DeleteOrder(DeleteOrderRequest request)
        {
            try
            {
                await DeleteOrderAsync(request);
                return new BaseResponseModel<DeleteOrderResponse>
                {
                    Code = 200,
                    Message = "Order deleted successfully",
                    Data = new DeleteOrderResponse { Success = true }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<DeleteOrderResponse>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = new DeleteOrderResponse { Success = false }
                };
            }
        }

        #endregion

        #region Private Methods

        private async Task<List<GetOrderByUserIdResponse>> GetOrdersByUserIdAsync(GetOrdersByUserIdRequest request)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(request.UserId);
            if (orders == null || !orders.Any())
            {
                throw new InvalidOperationException($"No orders found for user with ID {request.UserId}");
            }

            return orders.Select(o => new GetOrderByUserIdResponse
            {
                Id = o.Id,
                UserId = o.UserId,
                ShippingInfoId = o.ShippingInfoId,
                VoucherId = o.VoucherId,
                TotalPrice = o.TotalPrice,
                Status = o.Status,
                Note = null,
                CreatedAt = o.CreatedAt,
                UpdatedAt = o.UpdatedAt,
                OrderDetails = o.OrderDetails?.Select(od => new OrderDetailDto
                {
                    CustomShoeDesignId = od.CustomShoeDesignId,
                    Quantity = od.Quantity,
                    UnitPrice = od.Price,
                    Size = null
                }).ToList() ?? new List<OrderDetailDto>()
            }).ToList();
        }

        private async Task<GetOrderByIdResponse> GetOrderByIdAsync(GetOrderByIdRequest request)
        {
            var order = await _orderRepository.FindAsync(request.Id);
            if (order == null)
            {
                throw new InvalidOperationException($"Order with ID {request.Id} not found");
            }

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
                OrderDetails = order.OrderDetails?.Select(od => new OrderDetailDto
                {
                    CustomShoeDesignId = od.CustomShoeDesignId,
                    Quantity = od.Quantity,
                    UnitPrice = od.Price,
                    Size = null
                }).ToList() ?? new List<OrderDetailDto>()
            };
        }

        private async Task<List<GetOrderByIdResponse>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            if (orders == null || !orders.Any())
            {
                throw new InvalidOperationException("No orders found");
            }

            return orders.Select(o => new GetOrderByIdResponse
            {
                Id = o.Id,
                UserId = o.UserId,
                ShippingInfoId = o.ShippingInfoId,
                VoucherId = o.VoucherId,
                TotalPrice = o.TotalPrice,
                Status = o.Status,
                Note = null,
                CreatedAt = o.CreatedAt,
                UpdatedAt = o.UpdatedAt,
                OrderDetails = o.OrderDetails?.Select(od => new OrderDetailDto
                {
                    CustomShoeDesignId = od.CustomShoeDesignId,
                    Quantity = od.Quantity,
                    UnitPrice = od.Price,
                    Size = null
                }).ToList() ?? new List<OrderDetailDto>()
            }).ToList();
        }

        private async Task<AddOrderResponse> AddOrderAsync(AddOrderRequest request)
        {
            var order = new Order
            {
                UserId = request.UserId,
                ShippingInfoId = request.ShippingInfoId,
                VoucherId = request.VoucherId,
                TotalPrice = request.TotalPrice,
                Status = OrderStatus.Pending,
                ShippingStatus = OrderShippingStatus.Preparing,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var addedOrder = await _orderRepository.AddAsync(order);

            if (request.OrderDetails?.Any() == true)
            {
                foreach (var od in request.OrderDetails)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderId = addedOrder.Id,
                        CustomShoeDesignId = od.CustomShoeDesignId,
                        Quantity = od.Quantity,
                        Price = od.UnitPrice,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    await _orderDetailRepository.AddAsync(orderDetail);
                }
            }

            return new AddOrderResponse
            {
                Id = addedOrder.Id,
                TotalPrice = addedOrder.TotalPrice,
                Status = addedOrder.Status
            };
        }

        private async Task<UpdateOrderResponse> UpdateOrderAsync(UpdateOrderRequest request)
        {
            var order = await _orderRepository.FindAsync(request.Id);
            if (order == null)
            {
                throw new InvalidOperationException($"Order with ID {request.Id} not found");
            }

            order.ShippingInfoId = request.ShippingInfoId;
            order.VoucherId = request.VoucherId;
            order.TotalPrice = request.TotalPrice;
            order.Status = request.Status;
            order.UpdatedAt = DateTime.UtcNow;

            var existingOrderDetails = await _orderDetailRepository.GetByOrderIdAsync(order.Id);
            foreach (var existingDetail in existingOrderDetails)
            {
                await _orderDetailRepository.DeleteAsync(existingDetail.Id);
            }

            if (request.OrderDetails?.Any() == true)
            {
                foreach (var od in request.OrderDetails)
                {
                    var newOrderDetail = new OrderDetail
                    {
                        OrderId = order.Id,
                        CustomShoeDesignId = od.CustomShoeDesignId,
                        Quantity = od.Quantity,
                        Price = od.UnitPrice,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    await _orderDetailRepository.AddAsync(newOrderDetail);
                }
            }

            await _orderRepository.UpdateAsync(order);

            return new UpdateOrderResponse
            {
                Id = order.Id,
                TotalPrice = order.TotalPrice,
                Status = order.Status
            };
        }

        private async Task DeleteOrderAsync(DeleteOrderRequest request)
        {
            var order = await _orderRepository.FindAsync(request.Id);
            if (order == null)
            {
                throw new InvalidOperationException($"Order with ID {request.Id} not found");
            }

            var orderDetails = await _orderDetailRepository.GetByOrderIdAsync(request.Id);
            foreach (var orderDetail in orderDetails)
            {
                await _orderDetailRepository.DeleteAsync(orderDetail.Id);
            }

            await _orderRepository.DeleteAsync(order.Id);
        }

        #endregion
    }
}