using FCSP.DTOs;
using FCSP.DTOs.OrderDetail;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace FCSP.Services.OrderDetailService
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomShoeDesignRepository _customShoeDesignRepository;
        private readonly ISizeRepository _sizeRepository;
        private readonly IManufacturerRepository _manufacturerRepository;

        public OrderDetailService(
            IOrderDetailRepository orderDetailRepository, 
            IOrderRepository orderRepository, 
            ICustomShoeDesignRepository customShoeDesignRepository, 
            ISizeRepository sizeRepository,
            IManufacturerRepository manufacturerRepository)
        {
            _orderDetailRepository = orderDetailRepository;
            _orderRepository = orderRepository;
            _customShoeDesignRepository = customShoeDesignRepository;
            _sizeRepository = sizeRepository;
            _manufacturerRepository = manufacturerRepository;
        }

        public async Task<BaseResponseModel<IEnumerable<GetOrderDetailByIdResponse>>> GetAllOrderDetails()
        {
            try
            {
                var response = await _orderDetailRepository.GetAll()
                    .Include(od => od.Size)
                    .Include(od => od.Manufacturer)
                    .ToListAsync();
                
                return new BaseResponseModel<IEnumerable<GetOrderDetailByIdResponse>>
                {
                    Code = 200,
                    Message = "Order details retrieved successfully",
                    Data = response.Select(orderDetail => new GetOrderDetailByIdResponse
                    {
                        Id = orderDetail.Id,
                        OrderId = orderDetail.OrderId,
                        CustomShoeDesignName = orderDetail.CustomShoeDesign.Name,
                        Quantity = orderDetail.Quantity,
                        UnitPrice = orderDetail.Price,
                        SizeId = orderDetail.SizeId,
                        ManufacturerId = orderDetail.ManufacturerId,
                        CreatedAt = orderDetail.CreatedAt,
                        UpdatedAt = orderDetail.UpdatedAt
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<IEnumerable<GetOrderDetailByIdResponse>>
                {
                    Code = 500,
                    Message = $"Error retrieving order details: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<AddOrderDetailResponse>> AddOrderDetail(AddOrderDetailRequest request)
        {
            try
            {
                var orderDetail = await GetEntityFromAddRequest(request);
                var addedOrderDetail = await _orderDetailRepository.AddAsync(orderDetail);
                return new BaseResponseModel<AddOrderDetailResponse>
                {
                    Code = 200,
                    Message = "Order detail added successfully",
                    Data = new AddOrderDetailResponse
                    {
                        Success = true
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<AddOrderDetailResponse>
                {
                    Code = 500,
                    Message = $"Error adding order detail: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<UpdateOrderDetailResponse>> UpdateOrderDetail(UpdateOrderDetailRequest request)
        {
            try
            {
                OrderDetail orderDetail = await GetEntityFromUpdateRequest(request);
                await _orderDetailRepository.UpdateAsync(orderDetail);
                return new BaseResponseModel<UpdateOrderDetailResponse>
                {
                    Code = 200,
                    Message = "Order detail updated successfully",
                    Data = new UpdateOrderDetailResponse
                    {
                        Id = orderDetail.Id,
                        UnitPrice = orderDetail.Price
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<UpdateOrderDetailResponse>
                {
                    Code = 500,
                    Message = $"Error updating order detail: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<DeleteOrderDetailResponse>> DeleteOrderDetail(DeleteOrderDetailRequest request)
        {
            try
            {
                var orderDetail = await GetEntityFromDeleteRequest(request);
                await _orderDetailRepository.DeleteAsync(orderDetail.Id);
                return new BaseResponseModel<DeleteOrderDetailResponse>
                {
                    Code = 200,
                    Message = "Order detail deleted successfully",
                    Data = new DeleteOrderDetailResponse { Success = true }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<DeleteOrderDetailResponse>
                {
                    Code = 500,
                    Message = $"Error deleting order detail: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<BaseResponseModel<GetOrderDetailByIdResponse>> GetOrderDetailById(GetOrderDetailByIdRequest request)
        {
            try
            {
                var orderDetails = await GetEntityFromGetByIdRequest(request);
                return new BaseResponseModel<GetOrderDetailByIdResponse>
                {
                    Code = 200,
                    Message = "Order details retrieved successfully",
                    Data = new GetOrderDetailByIdResponse
                    {
                        Id = orderDetails.Id,
                        OrderId = orderDetails.OrderId,
                        CustomShoeDesignName = orderDetails.CustomShoeDesign.Name,
                        Quantity = orderDetails.Quantity,
                        UnitPrice = orderDetails.Price,
                        SizeId = orderDetails.SizeId,
                        ManufacturerId = orderDetails.ManufacturerId,
                        CreatedAt = orderDetails.CreatedAt,
                        UpdatedAt = orderDetails.UpdatedAt
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<GetOrderDetailByIdResponse>
                {
                    Code = 500,
                    Message = $"Error retrieving order details: {ex.Message}",
                    Data = null
                };
            }
        }

        private async Task<OrderDetail> GetEntityFromGetByIdRequest(GetOrderDetailByIdRequest request)
        {
            var orderDetail = await _orderDetailRepository.GetAll()
                                                            .Include(od => od.Size)
                                                            .Include(od => od.Manufacturer)
                                                            .Include(od => od.CustomShoeDesign)
                                                                .ThenInclude(cd => cd.CustomShoeDesignTemplate)
                                                            .Include(od => od.CustomShoeDesign)
                                                                .ThenInclude(cd => cd.DesignPreviews)
                                                            .Include(od => od.CustomShoeDesign)
                                                                .ThenInclude(cd => cd.DesignServices)
                                                                    .ThenInclude(ds => ds.Service)
                                                            .FirstOrDefaultAsync(od => od.Id == request.Id);
            if (orderDetail == null)
            {
                return null;
            }
            return orderDetail;
        }

        private async Task<OrderDetail> GetEntityFromAddRequest(AddOrderDetailRequest request)
        {
            var customShoeDesign = await _customShoeDesignRepository.FindAsync(request.CustomShoeDesignId);
            if (customShoeDesign == null)
            {
                return null;
            }

            var size = await _sizeRepository.FindAsync(request.SizeId);
            if (size == null)
            {
                return null;
            }

            var order = await _orderRepository.FindAsync(request.OrderId);
            if (order == null)
            {
                return null;
            }

            var manufacturer = await _manufacturerRepository.FindAsync(request.ManufacturerId);
            if (manufacturer == null)
            {
                return null;
            }

            return new OrderDetail
            {
                OrderId = request.OrderId,
                CustomShoeDesignId = customShoeDesign.Id,
                Quantity = request.Quantity,
                Price = customShoeDesign.TotalAmount,
                SizeId = size.Id,
                ManufacturerId = request.ManufacturerId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }

        private async Task<OrderDetail> GetEntityFromUpdateRequest(UpdateOrderDetailRequest request)
        {
            var orderDetail = await _orderDetailRepository.FindAsync(request.Id);
            if (orderDetail == null)
            {
                return null;
            }

            var manufacturer = await _manufacturerRepository.FindAsync(request.ManufacturerId);
            if (manufacturer == null)
            {
                return null;
            }

            orderDetail.Quantity = request.Quantity;
            orderDetail.SizeId = request.SizeId;
            orderDetail.ManufacturerId = request.ManufacturerId;
            orderDetail.UpdatedAt = DateTime.UtcNow;

            return orderDetail;
        }

        private async Task<OrderDetail> GetEntityFromDeleteRequest(DeleteOrderDetailRequest request)
        {
            var orderDetail = await _orderDetailRepository.FindAsync(request.Id);
            if (orderDetail == null)
            {
                return null;
            }
            return orderDetail;
        }
    }
}