namespace E_Commerce.Services.OrderServices
{
    using AutoMapper;
    using E_Commerce.DTOs;
    using E_Commerce.Models;
    using E_Commerce.Data;
    using Microsoft.EntityFrameworkCore;
    using E_Commerce.Helpers;

    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public OrderService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // 1. Create a new order
        public async Task<ApiResponses<string>> CreateOrder(OrderCreateDTO orderCreateDTO, Guid userId)
        {
            var cart = await _context.Carts.Include(c => c.CartItems)
                                           .ThenInclude(ci => ci.Product)
                                           .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || !cart.CartItems.Any())
            {
                return new ApiResponses<string>(404, "Cart is empty or not found.");
            }

            // Validate stock levels
            foreach (var cartItem in cart.CartItems)
            {
                if (cartItem.Quantity > cartItem.Product.Stock)
                {
                    return new ApiResponses<string>(400, $"Insufficient stock for product: {cartItem.Product.Name}.");
                }
            }

            // Calculate total amount
            var totalAmount = cart.CartItems.Sum(ci => ci.Quantity * ci.Product.Price);

            // Map OrderCreateDTO to Order
            var order = _mapper.Map<Order>(orderCreateDTO);
            order.UserId = userId;
            order.TotalAmount = totalAmount;
            order.OrderItems = cart.CartItems.Select(ci => new OrderItem
            {
                ProductId = ci.ProductId,
                Quantity = ci.Quantity,
                TotalPrice = ci.Quantity * ci.Product.Price
            }).ToList();

            // Save the order and update stock levels
            _context.Orders.Add(order);

            foreach (var cartItem in cart.CartItems)
            {
                cartItem.Product.Stock -= cartItem.Quantity;
            }

            _context.CartItems.RemoveRange(cart.CartItems); // Clear cart after order
            await _context.SaveChangesAsync();

            return new ApiResponses<string>(201, "Order created successfully.");
        }


        // 2. Retrieve orders for a user
        public async Task<ApiResponses<ICollection<OrderGetDTO>>> GetOrders(Guid userId)
        {
            var orders = await _context.Orders
                                        .Include(o => o.OrderItems)
                                        .ThenInclude(oi => oi.Product)
                                        .Where(o => o.UserId == userId)
                                        .ToListAsync();

            if (!orders.Any())
            {
                return new ApiResponses<ICollection<OrderGetDTO>>(404, "No orders found for the user.");
            }

            var orderDTOs = _mapper.Map<ICollection<OrderGetDTO>>(orders);
            return new ApiResponses<ICollection<OrderGetDTO>>(200, "Orders retrieved successfully.", orderDTOs);
        }

        // 3. Update order status
        public async Task<ApiResponses<string>> UpdateOrderStatus(Guid orderId, string newStatus)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return new ApiResponses<string>(404, "Order not found.");
            }

            order.Status = newStatus;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return new ApiResponses<string>(200, "Order status updated successfully.");
        }

        // 4. Delete/cancel an order
        public async Task<ApiResponses<string>> DeleteOrder(Guid orderId)
        {
            var order = await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return new ApiResponses<string>(404, "Order not found.");
            }

            _context.OrderItems.RemoveRange(order.OrderItems); // Remove associated OrderItems
            _context.Orders.Remove(order); // Remove the order
            await _context.SaveChangesAsync();

            return new ApiResponses<string>(200, "Order deleted successfully.");
        }

        // Admin: View all orders
        public async Task<ApiResponses<ICollection<OrderGetDTO>>> GetAllOrders()
        {
            var orders = await _context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.Product).ToListAsync();

            if (!orders.Any())
            {
                return new ApiResponses<ICollection<OrderGetDTO>>(404, "No orders found.");
            }

            var orderDTOs = _mapper.Map<ICollection<OrderGetDTO>>(orders);
            return new ApiResponses<ICollection<OrderGetDTO>>(200, "Orders retrieved successfully.", orderDTOs);
        }

        // Admin: View orders of a specific user
        public async Task<ApiResponses<ICollection<OrderGetDTO>>> GetOrdersByUser(Guid userId)
        {
            var orders = await _context.Orders
                                        .Include(o => o.OrderItems)
                                        .ThenInclude(oi => oi.Product)
                                        .Where(o => o.UserId == userId)
                                        .ToListAsync();

            if (!orders.Any())
            {
                return new ApiResponses<ICollection<OrderGetDTO>>(404, "No orders found for the specified user.");
            }

            var orderDTOs = _mapper.Map<ICollection<OrderGetDTO>>(orders);
            return new ApiResponses<ICollection<OrderGetDTO>>(200, "Orders for the user retrieved successfully.", orderDTOs);
        }

        // Admin: Calculate total revenue
        public async Task<ApiResponses<decimal>> GetTotalRevenue()
        {
            var totalRevenue = await _context.Orders.Where(o => o.Status == "Delivered").SumAsync(o => o.TotalAmount);
            return new ApiResponses<decimal>(200, "Total revenue calculated successfully.", totalRevenue);
        }

        // Admin: Calculate total products purchased
        public async Task<ApiResponses<int>> GetTotalProductsPurchased()
        {
            var totalProducts = await _context.OrderItems.Where(oi => oi.Order.Status == "Delivered").SumAsync(oi => oi.Quantity);
            return new ApiResponses<int>(200, "Total products purchased calculated successfully.", totalProducts);
        }
    }

}
