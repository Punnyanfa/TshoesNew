using FCSP.DTOs;
using FCSP.DTOs.Cart;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using System;
using System.Linq;

namespace FCSP.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly ICustomShoeDesignRepository _customShoeDesignRepository;

        public CartService(
            ICartRepository cartRepository,
            ICartItemRepository cartItemRepository,
            ICustomShoeDesignRepository customShoeDesignRepository)
        {
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _customShoeDesignRepository = customShoeDesignRepository;
        }

        public async Task<BaseResponseModel<GetCartResponse>> GetCartByUserIdAsync(long userId)
        {
            try
            {
                var response = new BaseResponseModel<GetCartResponse>();

                var cart = await _cartRepository.GetCartWithItemsByUserIdAsync(userId);
                if (cart == null)
                {
                    response.Message = "Cart not found for this user";
                    return response;
                }

                var cartItems = new List<CartItemDto>();
                foreach (var ci in cart.CartItems)
                {
                    var price = await _customShoeDesignRepository.GetTotalAmountByIdAsync(ci.CustomShoeDesignId);
                    cartItems.Add(new CartItemDto
                    {
                        Id = ci.Id,
                        CustomShoeDesignId = ci.CustomShoeDesignId,
                        CustomShoeDesignName = ci.CustomShoeDesign?.Name ?? "Unknown Design",
                        Price = price,
                        Quantity = ci.Quantity
                    });
                }

                var cartResponse = new GetCartResponse
                {
                    Id = cart.Id,
                    UserId = cart.UserId,
                    CartItems = cartItems
                };

                return new BaseResponseModel<GetCartResponse>
                {
                    Code = 200,
                    Data = cartResponse,
                    Message = "Cart retrieved successfully"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<GetCartResponse>
                {
                    Code = 500,
                    Message = $"Error retrieving cart: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponseModel<AddToCartResponse>> AddToCartAsync(AddToCartRequest request)
        {
            try
            {
                var response = new BaseResponseModel<AddToCartResponse>();

                // Check if the design exists
                var design = await _customShoeDesignRepository.FindAsync(request.CustomShoeDesignId);
                if (design == null || design.IsDeleted)
                {
                    response.Message = "Custom shoe design not found";
                    return response;
                }

                // Get or create cart for the user
                var cart = await _cartRepository.GetCartByUserIdAsync(request.UserId);
                if (cart == null)
                {
                    cart = new Cart
                    {
                        UserId = request.UserId,
                        IsDeleted = false
                    };
                    cart = await _cartRepository.AddAsync(cart);
                }

                // Check if the item is already in the cart
                var existingItem = await _cartItemRepository.GetCartItemByDesignIdAndCartIdAsync(
                    request.CustomShoeDesignId, cart.Id);

                CartItem cartItem;
                if (existingItem != null)
                {
                    // Update quantity of existing item
                    existingItem.Quantity += request.Quantity;
                    await _cartItemRepository.UpdateAsync(existingItem);
                    cartItem = existingItem;
                }
                else
                {
                    // Add new item
                    cartItem = new CartItem
                    {
                        CartId = cart.Id,
                        CustomShoeDesignId = request.CustomShoeDesignId,
                        Quantity = request.Quantity,
                        IsDeleted = false
                    };
                    cartItem = await _cartItemRepository.AddAsync(cartItem);
                }

                // Get total items in cart
                var cartItems = await _cartItemRepository.GetCartItemsByCartIdAsync(cart.Id);
                int totalItems = cartItems.Sum(i => i.Quantity);

                return new BaseResponseModel<AddToCartResponse>
                {
                    Code = 200,
                    Data = new AddToCartResponse
                    {
                        CartId = cart.Id,
                        CartItemId = cartItem.Id,
                        TotalItems = totalItems
                    },
                    Message = "Item added to cart successfully"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<AddToCartResponse>
                {
                    Code = 500,
                    Message = $"Error adding item to cart: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponseModel<UpdateCartItemResponse>> UpdateCartItemAsync(UpdateCartItemRequest request)
        {
            try
            {
                var response = new BaseResponseModel<UpdateCartItemResponse>();

                // Get cart for user
                var cart = await _cartRepository.GetCartWithItemsByUserIdAsync(request.UserId);
                if (cart == null)
                {
                    response.Message = "Cart not found for this user";
                    return response;
                }

                // Find the cart item
                var cartItem = await _cartItemRepository.FindAsync(request.CartItemId);
                if (cartItem == null || cartItem.CartId != cart.Id || cartItem.IsDeleted)
                {
                    response.Message = "Cart item not found";
                    return response;
                }

                if (request.Quantity <= 0)
                {
                    // If quantity is 0 or negative, remove the item
                    await RemoveCartItemAsync(request.CartItemId);

                    // Recalculate cart totals
                    var updatedCart = await _cartRepository.GetCartWithItemsByUserIdAsync(request.UserId);
                    float cartTotal = updatedCart?.CartItems.Sum(i => i.CustomShoeDesign.TotalAmount * i.Quantity) ?? 0;

                    response.Data = new UpdateCartItemResponse
                    {
                        Success = true,
                        CartItemId = request.CartItemId,
                        Quantity = request.Quantity,
                        Subtotal = 0,
                        CartTotal = cartTotal
                    };
                }
                else
                {
                    // Update the quantity
                    cartItem.Quantity = request.Quantity;
                    await _cartItemRepository.UpdateAsync(cartItem);

                    float subtotal = cartItem.CustomShoeDesign.TotalAmount * cartItem.Quantity;

                    // Calculate total cart value
                    var updatedCart = await _cartRepository.GetCartWithItemsByUserIdAsync(request.UserId);
                    float cartTotal = updatedCart.CartItems.Sum(i => i.CustomShoeDesign.TotalAmount * i.Quantity);

                    response.Data = new UpdateCartItemResponse
                    {
                        Success = true,
                        CartItemId = cartItem.Id,
                        Quantity = cartItem.Quantity,
                        Subtotal = subtotal,
                        CartTotal = cartTotal
                    };
                }

                return new BaseResponseModel<UpdateCartItemResponse>
                {
                    Code = 200,
                    Data = response.Data,
                    Message = "Cart item updated successfully"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<UpdateCartItemResponse>
                {
                    Code = 500,
                    Message = $"Error updating cart item: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponseModel<RemoveFromCartResponse>> RemoveFromCartAsync(RemoveFromCartRequest request)
        {
            try
            {
                var response = new BaseResponseModel<RemoveFromCartResponse>();

                // Get cart for user
                var cart = await _cartRepository.GetCartWithItemsByUserIdAsync(request.UserId);
                if (cart == null)
                {
                    response.Message = "Cart not found for this user";
                    return response;
                }

                // Find the cart item
                var cartItem = await _cartItemRepository.FindAsync(request.CartItemId);
                if (cartItem == null || cartItem.CartId != cart.Id || cartItem.IsDeleted)
                {
                    response.Message = "Cart item not found";
                    return response;
                }

                // Remove the item
                await RemoveCartItemAsync(request.CartItemId);

                // Recalculate cart totals
                var updatedCart = await _cartRepository.GetCartWithItemsByUserIdAsync(request.UserId);
                int remainingItems = updatedCart?.CartItems.Sum(i => i.Quantity) ?? 0;
                float cartTotal = updatedCart?.CartItems.Sum(i => i.CustomShoeDesign.TotalAmount * i.Quantity) ?? 0;

                response.Data = new RemoveFromCartResponse
                {
                    Success = true,
                    RemainingItems = remainingItems,
                    CartTotal = cartTotal
                };

                return new BaseResponseModel<RemoveFromCartResponse>
                {
                    Code = 200,
                    Data = response.Data,
                    Message = "Item removed from cart successfully"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<RemoveFromCartResponse>
                {
                    Code = 500,
                    Message = $"Error removing item from cart: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponseModel<bool>> ClearCartAsync(long userId)
        {
            try
            {
                var response = new BaseResponseModel<bool>();

                // Get cart for user
                var cart = await _cartRepository.GetCartWithItemsByUserIdAsync(userId);
                if (cart == null)
                {
                    response.Message = "Cart not found for this user";
                    return response;
                }

                // Soft delete all cart items
                foreach (var item in cart.CartItems)
                {
                    await RemoveCartItemAsync(item.Id);
                }

                return new BaseResponseModel<bool>
                {
                    Code = 200,
                    Data = true,
                    Message = "Cart cleared successfully"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel<bool>
                {
                    Code = 500,
                    Message = $"Error clearing cart: {ex.Message}"
                };
            }
        }

        private async Task RemoveCartItemAsync(long cartItemId)
        {
            var cartItem = await _cartItemRepository.FindAsync(cartItemId);
            if (cartItem != null)
            {
                cartItem.IsDeleted = true;
                await _cartItemRepository.UpdateAsync(cartItem);
            }
        }
    }
}