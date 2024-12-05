using E_Commerce.DTOs;
using E_Commerce.Services;
using E_Commerce.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListController : ControllerBase
    {
        private readonly IWishListService _wishListService;

        public WishListController(IWishListService wishListService)
        {
            _wishListService = wishListService;
        }

        [HttpGet("GetWishList")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetWishList()
        {
            try
            {
                Guid userId = Guid.Parse(HttpContext.Items["UserId"]?.ToString());
                var res = await _wishListService.GetWishList(userId);
                return Ok(new ApiResponses<List<WishListViewDTO>>(200, "Wishlist fetched correctly", res));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500, "Failed to fetch Wishlist", null, ex.InnerException?.Message));
            }
        }

        [HttpPost("AddOrRemove/{productId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> AddOrRemove(Guid productId)
        {
            try
            {
                Guid userId = Guid.Parse(HttpContext.Items["UserId"]?.ToString());
                string res = await _wishListService.AddOrRemove(userId, productId);
                return Ok(new ApiResponses<string>(200, res));

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500, "Operation on wishlist failed", null, ex.InnerException.Message));
            }
        }
    }
}
