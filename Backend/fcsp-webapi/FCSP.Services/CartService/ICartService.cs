using FCSP.DTOs;
using FCSP.DTOs.Cart;

namespace FCSP.Services.CartService
{
    public interface ICartService
    {
        Task<BaseResponseModel<GetCartResponse>> GetCartByUserIdAsync(long userId);
        Task<BaseResponseModel<AddToCartResponse>> AddToCartAsync(AddToCartRequest request);
        Task<BaseResponseModel<UpdateCartItemResponse>> UpdateCartItemAsync(UpdateCartItemRequest request);
        Task<BaseResponseModel<RemoveFromCartResponse>> RemoveFromCartAsync(RemoveFromCartRequest request);
        Task<BaseResponseModel<bool>> ClearCartAsync(long userId);
    }
} 