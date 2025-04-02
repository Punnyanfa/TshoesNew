using FCSP.DTOs.OrderDetail;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FCSP.Services.OrderDetailService
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IOrderDetailRepository _orderDetailRepository;

        public OrderDetailService(IOrderDetailRepository orderDetailRepository)
        {
            _orderDetailRepository = orderDetailRepository;
        }

        public async Task<IEnumerable<OrderDetail>> GetAllOrderDetails()
        {
            var response = await _orderDetailRepository.GetAllAsync();
            return response;
        }

        public async Task<GetOrderDetailByIdResponse> GetOrderDetailById(GetOrderDetailByIdRequest request)
        {
            OrderDetail orderDetail = await GetEntityFromGetByIdRequest(request);
            return new GetOrderDetailByIdResponse
            {
                Id = orderDetail.Id,
                OrderId = orderDetail.OrderId,
                CustomShoeDesignId = orderDetail.CustomShoeDesignId,
                Quantity = orderDetail.Quantity,
                UnitPrice = orderDetail.Price,
                Size = null,
                CreatedAt = orderDetail.CreatedAt,
                UpdatedAt = orderDetail.UpdatedAt
            };
        }

        public async Task<AddOrderDetailResponse> AddOrderDetail(AddOrderDetailRequest request)
        {
            OrderDetail orderDetail = GetEntityFromAddRequest(request);
            var addedOrderDetail = await _orderDetailRepository.AddAsync(orderDetail);
            return new AddOrderDetailResponse
            {
                Id = addedOrderDetail.Id,
                UnitPrice = addedOrderDetail.Price
            };
        }

        public async Task<UpdateOrderDetailResponse> UpdateOrderDetail(UpdateOrderDetailRequest request)
        {
            OrderDetail orderDetail = await GetEntityFromUpdateRequest(request);
            await _orderDetailRepository.UpdateAsync(orderDetail);
            return new UpdateOrderDetailResponse
            {
                Id = orderDetail.Id,
                UnitPrice = orderDetail.Price
            };
        }

        public async Task<DeleteOrderDetailResponse> DeleteOrderDetail(DeleteOrderDetailRequest request)
        {
            OrderDetail orderDetail = await GetEntityFromDeleteRequest(request);
            await _orderDetailRepository.DeleteAsync(orderDetail.Id);
            return new DeleteOrderDetailResponse { Success = true };
        }

        private async Task<OrderDetail> GetEntityFromGetByIdRequest(GetOrderDetailByIdRequest request)
        {
            OrderDetail orderDetail = await _orderDetailRepository.FindAsync(request.Id);
            if (orderDetail == null)
            {
                throw new InvalidOperationException("OrderDetail not found");
            }
            return orderDetail;
        }

        private OrderDetail GetEntityFromAddRequest(AddOrderDetailRequest request)
        {
            return new OrderDetail
            {
                OrderId = request.OrderId,
                CustomShoeDesignId = request.CustomShoeDesignId,
                Quantity = request.Quantity,
                Price = request.UnitPrice,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }

        private async Task<OrderDetail> GetEntityFromUpdateRequest(UpdateOrderDetailRequest request)
        {
            OrderDetail orderDetail = await _orderDetailRepository.FindAsync(request.Id);
            if (orderDetail == null)
            {
                throw new InvalidOperationException("OrderDetail not found");
            }

            orderDetail.Quantity = request.Quantity;
            orderDetail.Price = request.UnitPrice;
            orderDetail.UpdatedAt = DateTime.UtcNow;

            return orderDetail;
        }

        private async Task<OrderDetail> GetEntityFromDeleteRequest(DeleteOrderDetailRequest request)
        {
            OrderDetail orderDetail = await _orderDetailRepository.FindAsync(request.Id);
            if (orderDetail == null)
            {
                throw new InvalidOperationException("OrderDetail not found");
            }
            return orderDetail;
        }
    }
}