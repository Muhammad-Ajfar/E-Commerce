using AutoMapper;
using E_Commerce.Data;
using E_Commerce.DTOs;
using E_Commerce.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Services
{
    public interface IWishListService
    {
        Task<string> AddOrRemove(Guid userId, Guid productId);
        Task<List<WishListViewDTO>> GetWishList(Guid userId);
    }

    public class WishListService : IWishListService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public WishListService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<string> AddOrRemove(Guid userId, Guid productId)
        {
            var isExist = await _context.WishLists.Include(w => w.Product).FirstOrDefaultAsync(w => w.ProductId == productId && w.UserId == userId);
            if (isExist == null)
            {
                WishListDTO wishListDTO = new WishListDTO()
                {
                    UserId = userId,
                    ProductId = productId
                };

                var w = _mapper.Map<WishList>(wishListDTO);
                _context.WishLists.Add(w);
                await _context.SaveChangesAsync();
                return "Item added to Wishlist";
            }
            else
            {
                _context.WishLists.Remove(isExist);
                await _context.SaveChangesAsync();
                return "Item removed from Wishlist";
            }
        }

        public async Task<List<WishListViewDTO>> GetWishList(Guid userId)
        {
            try
            {
                var items =await _context.WishLists.Include(w => w.Product).ThenInclude(p => p.Category)
                    .Where(w => w.UserId == userId).ToListAsync();

                if (items != null)
                {
                    var p = items.Select(w => new WishListViewDTO
                    {
                        Id = w.Id,
                        ProductId = w.ProductId,
                        ProductName = w.Product.Name,
                        Price = w.Product.Price,
                        CategoryName = w.Product.Category.Name
                    }).ToList();

                    return p;
                }
                else
                {
                    return new List<WishListViewDTO>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
