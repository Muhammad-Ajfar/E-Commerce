using E_Commerce.DTOs;

namespace E_Commerce.Services.WishListServices
{
    public interface IWishListService
    {
        Task<string> AddOrRemove(Guid userId, Guid productId);
        Task<List<WishListViewDTO>> GetWishList(Guid userId);
    }
}
