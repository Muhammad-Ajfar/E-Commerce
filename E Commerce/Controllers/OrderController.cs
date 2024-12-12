using E_Commerce.DTOs;
using E_Commerce.Services.OrderServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // 1. Create a new order (User only)
        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDTO orderCreateDTO)
        {
            var userIdString = HttpContext.Items["UserId"]?.ToString();
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            {
                return Unauthorized(new { message = "Invalid or missing token." });
            }

            var response = await _orderService.CreateOrder(orderCreateDTO, userId);
            return StatusCode(response.StatusCode, response);
        }

        // 2. Get orders for a user (User only)
        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetUserOrders()
        {
            var userIdString = HttpContext.Items["UserId"]?.ToString();
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            {
                return Unauthorized(new { message = "Invalid or missing token." });
            }

            var response = await _orderService.GetOrders(userId);
            return StatusCode(response.StatusCode, response);
        }

        // 3. Update order status (Admin only)
        [HttpPut("{orderId:guid}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOrderStatus(Guid orderId, [FromBody] string newStatus)
        {
            var response = await _orderService.UpdateOrderStatus(orderId, newStatus);
            return StatusCode(response.StatusCode, response);
        }

        // 4. Delete/cancel an order (User or Admin)
        [HttpDelete("{orderId:guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteOrder(Guid orderId)
        {
            var userIdString = HttpContext.Items["UserId"]?.ToString();
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            {
                return Unauthorized(new { message = "Invalid or missing token." });
            }

            var response = await _orderService.DeleteOrder(orderId);
            return StatusCode(response.StatusCode, response);
        }

        // Admin: View all orders
        [HttpGet("admin/all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllOrders()
        {
            var response = await _orderService.GetAllOrders();
            return StatusCode(response.StatusCode, response);
        }

        // Admin: View orders of a specific user
        [HttpGet("admin/user/{userId:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetOrdersByUser(Guid userId)
        {
            var response = await _orderService.GetOrdersByUser(userId);
            return StatusCode(response.StatusCode, response);
        }

        // Admin: Get total revenue
        [HttpGet("admin/revenue")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetTotalRevenue()
        {
            var response = await _orderService.GetTotalRevenue();
            return StatusCode(response.StatusCode, response);
        }

        // Admin: Get total products purchased
        [HttpGet("admin/products-purchased")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetTotalProductsPurchased()
        {
            var response = await _orderService.GetTotalProductsPurchased();
            return StatusCode(response.StatusCode, response);
        }
    }
}
