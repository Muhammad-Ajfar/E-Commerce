using E_Commerce.Data;
using E_Commerce.DTOs;
using E_Commerce.Helpers;
using E_Commerce.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace E_Commerce.Services.CartServices
{
    public class CartService : ICartService
    {
        private readonly AppDbContext _context;

        public CartService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CartViewDTO>> GetCartItems(Guid userId)
        {
           
        
                if (userId == Guid.Empty)
                {
                    throw new Exception("User ID is empty.");
                }

                var cart = await _context.Carts.Include(c => c.CartItems).ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

                if (cart != null)
                {
                    return cart.CartItems.Select(c => new CartViewDTO
                    {
                        ProductId = c.ProductId,
                        ProductName = c.Product.Name,
                        ProductImage = c.Product.Image,
                        Price = c.Product.Price,
                        Quantity = c.Quantity,
                        TotalAmount = c.Product.Price * c.Quantity
                    }).ToList();

                }

                return new List<CartViewDTO>();

            

        }

        public async Task<ApiResponses<CartItem>> AddToCart(Guid userId, Guid productId)
        {
            try
            {
                var user = await _context.Users.Include(u => u.Cart)
                    .ThenInclude(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    return new ApiResponses<CartItem>(404, "User not found");
                }

                if (user.Cart == null)
                {
                    user.Cart = new Cart { UserId = userId, CartItems = new List<CartItem>() };
                    _context.Carts.Add(user.Cart);
                    await _context.SaveChangesAsync();
                }

                var check = user.Cart?.CartItems?.FirstOrDefault(ci => ci.ProductId == productId);
                if (check != null)
                {
                    return new ApiResponses<CartItem>(409, "Product already in cart");
                }

                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
                if (product?.Stock <= 0)
                {
                    return new ApiResponses<CartItem>(404, "Out of Stock");
                }

                var item = new CartItem
                {
                    CartId = user.Cart.Id,
                    ProductId = productId,
                    Quantity = 1
                };

                return new ApiResponses<CartItem>(200, "Successfully added to cart");

            }

            catch (Exception ex)
            {
                return new ApiResponses<CartItem>(500, "Internal server error", null, ex.Message);
            }
        }

        public async Task<bool> RemoveFromCart( Guid userId, Guid productId )
        {
            try
            {
                var user = await _context.Users.Include(u => u.Cart)
                    .ThenInclude(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    throw new Exception("User is not found");
                }

                var deleteItem = user?.Cart?.CartItems?.FirstOrDefault(ci => ci.ProductId == productId);
                if (deleteItem == null)
                {
                    return false;
                }

                user.Cart.CartItems.Remove(deleteItem);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<ApiResponses<CartItem>> IncreaseQuantity(Guid userId, Guid productId)
        {
            try
            {
                var user = await _context.Users.Include(u => u.Cart)
                    .ThenInclude(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    return new ApiResponses<CartItem>(404, "User not found");
                }

                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
                if (product == null)
                {
                    return new ApiResponses<CartItem>(404, "Product not found");
                }

                var item = user?.Cart?.CartItems?.FirstOrDefault(ci => ci.ProductId == productId);
                if (item == null)
                {
                    return new ApiResponses<CartItem>(404, "Product not found in the cart");
                }

                if (item.Quantity >= 10)
                {
                    return new ApiResponses<CartItem>(400, "Maximum quantity reached (10 items)");
                }

                if (product.Stock <= item.Quantity)
                {
                    return new ApiResponses<CartItem>(400, "Out of stock");
                }

                item.Quantity++;
                await _context.SaveChangesAsync();

                return new ApiResponses<CartItem>(200, "Quantity increased successfully", item);
            }
            catch (Exception ex)
            {
                return new ApiResponses<CartItem>(500, "Internal server error", null, ex.Message);
            }
        }

        public async Task<bool> DecreaseQuantity(Guid userId, Guid productId)
        {
            try
            {
                var user = await _context.Users.Include(u => u.Cart)
                    .ThenInclude(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    throw new Exception("User not found");
                }

                var item = user.Cart.CartItems.FirstOrDefault(p => p.ProductId == productId);
                if (item == null)
                {
                    return false;
                }
                if (item.Quantity > 1)
                {
                    item.Quantity--;
                }
                else
                {
                    item.Quantity = 1;
                }
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }


}

