using FCSP.DTOs;
using FCSP.DTOs.Cart;
using FCSP.Services.CartService;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FCSP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        /// <summary>
        /// Retrieves the cart for a specific user
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>Cart details including items and totals</returns>
        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseResponseModel<GetCartResponse>>> GetCart(long userId)
        {
            var response = await _cartService.GetCartByUserIdAsync(userId);
            return response.Data != null ? Ok(response) : NotFound(response);
        }

        /// <summary>
        /// Adds an item to the user's cart
        /// </summary>
        /// <param name="request">Add to cart request details</param>
        /// <returns>Result of adding item to cart</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResponseModel<AddToCartResponse>>> AddToCart([FromBody] AddToCartRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _cartService.AddToCartAsync(request);
            return response.Data?.Success == true ? Ok(response) : BadRequest(response);
        }

        /// <summary>
        /// Updates the quantity of an item in the cart
        /// </summary>
        /// <param name="request">Update cart item details</param>
        /// <returns>Result of updating cart item</returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResponseModel<UpdateCartItemResponse>>> UpdateCartItem([FromBody] UpdateCartItemRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _cartService.UpdateCartItemAsync(request);
            return response.Data?.Success == true ? Ok(response) : BadRequest(response);
        }

        /// <summary>
        /// Removes an item from the cart
        /// </summary>
        /// <param name="request">Remove item details</param>
        /// <returns>Result of removing item</returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResponseModel<RemoveFromCartResponse>>> RemoveFromCart([FromBody] RemoveFromCartRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _cartService.RemoveFromCartAsync(request);
            return response.Data?.Success == true ? Ok(response) : BadRequest(response);
        }

        /// <summary>
        /// Clears all items from the user's cart
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>Result of clearing cart</returns>
        [HttpDelete("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResponseModel<bool>>> ClearCart(long userId)
        {
            var response = await _cartService.ClearCartAsync(userId);
            return response.Data == true ? Ok(response) : BadRequest(response);
        }
    }
}