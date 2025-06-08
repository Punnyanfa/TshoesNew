using FCSP.DTOs;
using FCSP.DTOs.OrderDetail;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using FCSP.Common.Utils;

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
                        CustomShoeDesignName = orderDetail.CustomShoeDesign.Name ?? string.Empty,
                        Quantity = orderDetail.Quantity,
                        UnitPrice = orderDetail.TotalPrice,
                        TemplatePrice = orderDetail.TemplatePrice,
                        ServicePrice = orderDetail.ServicePrice,
                        DesignerMarkup = orderDetail.DesignerMarkup,
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

        public async Task<BaseResponseModel<GetOrderDetailByManufacturerIdResponse>> GetOrderDetailByManufacturerId(GetOrderDetailByManufacturerIdRequest request)
        {
            try
            {
                var response = await _orderDetailRepository.GetByManufacturerIdAsync(request.ManufacturerId);

                return new BaseResponseModel<GetOrderDetailByManufacturerIdResponse>
                {
                    Code = 200,
                    Message = "Order details retrieved successfully",
                    Data = new GetOrderDetailByManufacturerIdResponse
                    {
                        OrderDetails = response.Select(orderDetail => new GetOrderDetailByIdResponse
                        {
                            Id = orderDetail.Id,
                            OrderId = orderDetail.OrderId,
                            CustomShoeDesignName = orderDetail.CustomShoeDesign.Name ?? string.Empty,
                            Quantity = orderDetail.Quantity,
                            UnitPrice = orderDetail.TotalPrice,
                            TemplatePrice = orderDetail.TemplatePrice,
                            ServicePrice = orderDetail.ServicePrice,
                            DesignerMarkup = orderDetail.DesignerMarkup,
                            SizeId = orderDetail.SizeId,
                            ManufacturerId = orderDetail.ManufacturerId,
                            CreatedAt = orderDetail.CreatedAt,
                            UpdatedAt = orderDetail.UpdatedAt
                        }).ToList()
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<GetOrderDetailByManufacturerIdResponse>
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
                        UnitPrice = orderDetail.TotalPrice,
                        TemplatePrice = orderDetail.TemplatePrice,
                        ServicePrice = orderDetail.ServicePrice,
                        DesignerMarkup = orderDetail.DesignerMarkup
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
                        CustomShoeDesignName =  orderDetails.CustomShoeDesign.Name ?? string.Empty,
                        Quantity = orderDetails.Quantity,
                        UnitPrice = orderDetails.TotalPrice,
                        TemplatePrice = orderDetails.TemplatePrice,
                        ServicePrice = orderDetails.ServicePrice,
                        DesignerMarkup = orderDetails.DesignerMarkup,
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
                throw new InvalidOperationException($"Order detail with ID {request.Id} not found");
            }
            return orderDetail;
        }

        private async Task<OrderDetail> GetEntityFromAddRequest(AddOrderDetailRequest request)
        {
            var customShoeDesign = await _customShoeDesignRepository.GetAll()
                .Include(cd => cd.CustomShoeDesignTemplate)
                .Include(cd => cd.DesignServices)
                    .ThenInclude(ds => ds.Service)
                .FirstOrDefaultAsync(cd => cd.Id == request.CustomShoeDesignId);
            
            if (customShoeDesign == null)
            {
                throw new InvalidOperationException($"Custom shoe design with ID {request.CustomShoeDesignId} not found");
            }

            var size = await _sizeRepository.FindAsync(request.SizeId);
            if (size == null)
            {
                throw new InvalidOperationException($"Size with ID {request.SizeId} not found");
            }

            var order = await _orderRepository.FindAsync(request.OrderId);
            if (order == null)
            {
                throw new InvalidOperationException($"Order with ID {request.OrderId} not found");
            }

            var manufacturer = await _manufacturerRepository.FindAsync(request.ManufacturerId);
            if (manufacturer == null)
            {
                throw new InvalidOperationException($"Manufacturer with ID {request.ManufacturerId} not found");
            }

            // Get template price
            var templatePrice = customShoeDesign.CustomShoeDesignTemplate?.Price ?? 0;
            
            // Calculate services price
            int servicesPrice = 0;
            if (customShoeDesign.DesignServices != null && customShoeDesign.DesignServices.Any())
            {
                servicesPrice = customShoeDesign.DesignServices.Sum(ds => ds.Service != null ? ds.Service.Price : 0);
            }

            var totalAmount = templatePrice + servicesPrice + customShoeDesign.DesignerMarkup;

            return new OrderDetail
            {
                OrderId = request.OrderId,
                CustomShoeDesignId = customShoeDesign.Id,
                Quantity = request.Quantity,
                TotalPrice = totalAmount,
                SizeId = size.Id,
                ManufacturerId = request.ManufacturerId,
                TemplatePrice = templatePrice,
                ServicePrice = servicesPrice,
                DesignerMarkup = customShoeDesign.DesignerMarkup,
                CreatedAt = DateTimeUtils.GetCurrentGmtPlus7(),
                UpdatedAt = DateTimeUtils.GetCurrentGmtPlus7()
            };
        }

        private async Task<OrderDetail> GetEntityFromUpdateRequest(UpdateOrderDetailRequest request)
        {
            var orderDetail = await _orderDetailRepository.FindAsync(request.Id);
            if (orderDetail == null)
            {
                throw new InvalidOperationException($"Order detail with ID {request.Id} not found");
            }

            var manufacturer = await _manufacturerRepository.FindAsync(request.ManufacturerId);
            if (manufacturer == null)
            {
                throw new InvalidOperationException($"Manufacturer with ID {request.ManufacturerId} not found");
            }

            orderDetail.Quantity = request.Quantity;
            orderDetail.SizeId = request.SizeId;
            orderDetail.ManufacturerId = request.ManufacturerId;
            
            // Update template price if provided
            if (request.TemplatePrice.HasValue)
            {
                orderDetail.TemplatePrice = request.TemplatePrice.Value;
            }
            
            // Update service price if provided
            if (request.ServicePrice.HasValue)
            {
                orderDetail.ServicePrice = request.ServicePrice.Value;
            }
            
            // Update designer markup if provided
            if (request.DesignerMarkup.HasValue)
            {
                orderDetail.DesignerMarkup = request.DesignerMarkup.Value;
            }
            
            // Recalculate total price
            orderDetail.TotalPrice = orderDetail.TemplatePrice + orderDetail.ServicePrice + orderDetail.DesignerMarkup;
            
            orderDetail.UpdatedAt = DateTimeUtils.GetCurrentGmtPlus7();

            return orderDetail;
        }

        private async Task<OrderDetail> GetEntityFromDeleteRequest(DeleteOrderDetailRequest request)
        {
            var orderDetail = await _orderDetailRepository.FindAsync(request.Id);
            if (orderDetail == null)
            {
                throw new InvalidOperationException($"Order detail with ID {request.Id} not found");
            }
            return orderDetail;
        }
    }
}