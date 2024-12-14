using E_Commerce.DTOs;
using E_Commerce.Helpers;
using E_Commerce.Services.CartServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
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

        [HttpGet("All")]
        [Authorize]
        public async Task<IActionResult> GetItems()
        {
            try
            {
                Guid userId = Guid.Parse(HttpContext.Items["UserId"]?.ToString());

                var cart = await _cartService.GetCartItems(userId);
                if (cart.Count == 0)
                {
                    return Ok(new ApiResponses<IEnumerable<CartViewDTO>>(200, "Cart is empty", cart));
                }
                return Ok(new ApiResponses<IEnumerable<CartViewDTO>>(200, "Cart successfully fetched", cart));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500, "Internal server error", null, ex.Message));
            }
        }


        [HttpPost("add/{productId}")]
        [Authorize]
        public async Task<IActionResult> AddToCart(Guid productId)
        {
            try
            {
                Guid userId = Guid.Parse(HttpContext.Items["UserId"]?.ToString());


                var res = await _cartService.AddToCart(userId, productId);


                if (res.StatusCode == 200)
                {
                    return Ok(res);
                }
                else if (res.StatusCode == 404)
                {
                    return NotFound(new ApiResponses<string>(404, res.Message));
                }
                else if (res.StatusCode == 400)
                {
                    return Conflict(new ApiResponses<string>(400, res.Message));
                }
                return BadRequest(new ApiResponses<string>(400, "Bad request"));
            }
            catch (Exception ex)
            {

                return StatusCode(500, new ApiResponses<string>(500, "Internal server error", null, ex.Message));
            }
        }


        [HttpDelete("Delete/{productId}")]
        [Authorize]
        public async Task<IActionResult> RemoveCart(Guid productId)
        {
            try
            {
                Guid userId = Guid.Parse(HttpContext.Items["UserId"]?.ToString());

                bool res = await _cartService.RemoveFromCart(userId, productId);
                if (res == false)
                {
                    return BadRequest(new ApiResponses<string>(400, "Item is not found in cart", null, "Item is not found in cart"));
                }
                return Ok(new ApiResponses<string>(200, "Item successfully deleted"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500, "Internal server error", null, ex.Message));
            }
        }


        [HttpPut("IncreaseQty/{productId}")]
        [Authorize]
        public async Task<IActionResult> IncreaseQty(Guid productId)
        {
            try
            {

                Guid userId = Guid.Parse(HttpContext.Items["UserId"]?.ToString());

                var res = await _cartService.IncreaseQuantity(userId, productId);

                if (res.StatusCode == 404)
                {
                    return BadRequest(new ApiResponses<string>(404, res.Message, null, res.Error));
                }

                if (res.StatusCode == 400)
                {
                    return BadRequest(new ApiResponses<string>(400, res.Message, null, res.Error));
                }
                return Ok(new ApiResponses<string>(200, "Quantity increased successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500, "Internal server error", null, ex.Message));
            }
        }


        [HttpPut("DecreaseQty/{productId}")]
        [Authorize]
        public async Task<IActionResult> DecreaseQty(Guid productId)
        {
            try
            {
                Guid userId = Guid.Parse(HttpContext.Items["UserId"]?.ToString());

                bool res = await _cartService.DecreaseQuantity(userId, productId);
                if (res == false)
                {
                    return BadRequest(new ApiResponses<string>(400, "Item not found in the cart", null, "Item not found in the cart"));
                }
                return Ok(new ApiResponses<string>(200, "Qty decreased"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500, "Internal server error", null, ex.Message));
            }
        }


    }
}
