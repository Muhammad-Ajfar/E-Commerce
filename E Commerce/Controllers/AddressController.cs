using E_Commerce.DTOs;
using E_Commerce.Services.AddressServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        // 1. Get all addresses for a user
        [HttpGet]
        public async Task<IActionResult> GetAddresses()
        {
            var userIdString = HttpContext.Items["UserId"]?.ToString();
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            {
                return Unauthorized(new { message = "Invalid or missing token." });
            }

            var response = await _addressService.GetAddressesByUserId(userId);
            return StatusCode(response.StatusCode, response);
        }

        // 2. Add a new address
        [HttpPost]
        public async Task<IActionResult> AddAddress([FromBody] AddressCreateDTO addressCreateDTO)
        {
            var userIdString = HttpContext.Items["UserId"]?.ToString();
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            {
                return Unauthorized(new { message = "Invalid or missing token." });
            }

            var response = await _addressService.AddAddress(addressCreateDTO, userId);
            return StatusCode(response.StatusCode, response);
        }

        // 3. Remove an address
        [HttpDelete("{addressId}")]
        public async Task<IActionResult> RemoveAddress(Guid addressId)
        {
            var userIdString = HttpContext.Items["UserId"]?.ToString();
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            {
                return Unauthorized(new { message = "Invalid or missing token." });
            }

            var response = await _addressService.RemoveAddress(addressId, userId);
            return StatusCode(response.StatusCode, response);
        }

    }
}
