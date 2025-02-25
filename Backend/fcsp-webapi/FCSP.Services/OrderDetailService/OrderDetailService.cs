using FCSP.DTOs.OrderDetail;
using FCSP.Models.Entities;
using FCSP.Repositories.OrderDetail;

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
            OrderDetail orderDetail = GetEntityFromGetByIdRequest(request);
            return new GetOrderDetailByIdResponse
            {
                OrderId = orderDetail.OrderId,
                CustomShoeDesignId = orderDetail.CustomShoeDesignId,
                Quantity = orderDetail.Quantity,
                Price = orderDetail.Price
            };
        }

        public async Task<AddOrderDetailResponse> AddOrderDetail(AddOrderDetailRequest request)
        {
            OrderDetail orderDetail = GetEntityFromAddRequest(request);
            var addedOrderDetail = await _orderDetailRepository.AddAsync(orderDetail);
            return new AddOrderDetailResponse { OrderDetailId = addedOrderDetail.Id };
        }

        public async Task<AddOrderDetailResponse> UpdateOrderDetail(UpdateOrderDetailRequest request)
        {
            OrderDetail orderDetail = GetEntityFromUpdateRequest(request);
            await _orderDetailRepository.UpdateAsync(orderDetail);
            return new AddOrderDetailResponse { OrderDetailId = orderDetail.Id };
        }

        public async Task<AddOrderDetailResponse> DeleteOrderDetail(DeleteOrderDetailRequest request)
        {
            OrderDetail orderDetail = GetEntityFromDeleteRequest(request);
            await _orderDetailRepository.DeleteAsync(orderDetail.Id);
            return new AddOrderDetailResponse { OrderDetailId = orderDetail.Id };
        }

        private OrderDetail GetEntityFromGetByIdRequest(GetOrderDetailByIdRequest request)
        {
            OrderDetail orderDetail = _orderDetailRepository.Find(request.Id);
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
                Price = request.Price
            };
        }

        private OrderDetail GetEntityFromUpdateRequest(UpdateOrderDetailRequest request)
        {
            OrderDetail orderDetail = _orderDetailRepository.Find(request.Id);
            if (orderDetail == null)
            {
                throw new InvalidOperationException("OrderDetail not found");
            }
            
            orderDetail.OrderId = request.OrderId ?? orderDetail.OrderId;
            orderDetail.CustomShoeDesignId = request.CustomShoeDesignId ?? orderDetail.CustomShoeDesignId;
            orderDetail.Quantity = request.Quantity ?? orderDetail.Quantity;
            orderDetail.Price = request.Price ?? orderDetail.Price;
            orderDetail.UpdatedAt = DateTime.Now;
            
            return orderDetail;
        }

        private OrderDetail GetEntityFromDeleteRequest(DeleteOrderDetailRequest request)
        {
            OrderDetail orderDetail = _orderDetailRepository.Find(request.Id);
            if (orderDetail == null)
            {
                throw new InvalidOperationException("OrderDetail not found");
            }
            return orderDetail;
        }
    }
} 