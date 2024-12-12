using E_Commerce.DTOs;

namespace E_Commerce.Services.ProductServices
{
    public interface IProductService
    {
        Task AddProduct(ProductDTO productDTO, IFormFile image);
        Task<List<ProductGetDTO>> GetProducts();
        Task<ProductGetDTO> GetProductById(Guid id);
        Task<List<ProductGetDTO>> GetProductByCategory(string categoryName);
        Task<bool> DeleteProduct(Guid id);
        Task UpdateProduct(Guid id, ProductDTO productDto, IFormFile image);
        Task<List<ProductGetDTO>> SearchProduct(string search);
        Task<List<ProductGetDTO>> ProductPagination(int pagenumber = 1, int size = 10);
    }
}
