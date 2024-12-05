using E_Commerce.DTOs;
using E_Commerce.Helpers;
using E_Commerce.Models;

namespace E_Commerce.Services.CartServices
{
    public interface ICartService
    {
        Task<List<CartViewDTO>> GetCartItems(Guid userId);
        Task<ApiResponses<CartItem>> AddToCart(Guid userId, Guid productId);
        Task<bool> RemoveFromCart(Guid userId, Guid productId);
        Task<ApiResponses<CartItem>> IncreaseQuantity(Guid userId, Guid productId);
        Task<bool> DecreaseQuantity(Guid userId, Guid productId);

    }
}
