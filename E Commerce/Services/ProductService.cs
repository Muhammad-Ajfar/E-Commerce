using AutoMapper;
using E_Commerce.Data;
using E_Commerce.DTOs;
using E_Commerce.Helpers;
using E_Commerce.Models;
using E_Commerce.Services.CloudinaryServices;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace E_Commerce.Services
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

    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinaryService;

        public ProductService(AppDbContext context, IMapper mapper, ICloudinaryService cloudinaryService)
        {
            _context = context;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<List<ProductGetDTO>> GetProducts()
        {
            try
            {
                var products = await _context.Products.Include(x => x.Category).ToListAsync();
                if (products.Count > 0)
                {
                    return _mapper.Map<List<ProductGetDTO>>(products);
                }
                return new List<ProductGetDTO>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task AddProduct(ProductDTO productDTO, IFormFile image)
        {
            try
            {
                string imageUrl = await _cloudinaryService.UploadImageAsync(image);
                var product = _mapper.Map<Product>(productDTO);
                product.Image = imageUrl;

                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ProductGetDTO> GetProductById(Guid id)
        {
            try
            {
                var prd = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
                if (prd == null)
                {
                    return new ProductGetDTO();
                }
                return _mapper.Map<ProductGetDTO>(prd);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ProductGetDTO>> GetProductByCategory(string categoryName)
        {
            try
            {
                if (categoryName == "All")
                {
                    var prod = await _context.Products.Include(p => p.Category).ToListAsync();
                    return _mapper.Map<List<ProductGetDTO>>(prod);

                }
                var products = await _context.Products.Include(p => p.Category)
                    .Where(p => p.Category.Name == categoryName).ToListAsync();

                return _mapper.Map<List<ProductGetDTO>>(products);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteProduct(Guid id)
        {
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
                if (product != null)
                {
                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateProduct(Guid id, ProductDTO productDto, IFormFile image)
        {
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
                var categoryExists = await _context.Categories.FirstOrDefaultAsync(x => x.Name == productDto.CategoryName);
                if (categoryExists == null)
                {
                    throw new Exception("Category not found");
                }
                if (product != null)
                {
                    _mapper.Map<Product>(productDto);

                    if (image != null && image.Length > 0)
                    {
                        string imageUrl = await _cloudinaryService.UploadImageAsync(image);
                        product.Image = imageUrl;
                    }
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception($"Product with ID: {id} not found!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task<List<ProductGetDTO>> SearchProduct(string searchTerm)
        {


            if (string.IsNullOrEmpty(searchTerm))
            {
                return new List<ProductGetDTO>();
            }


            var products = await _context.Products.Include(x => x.Category)
                .Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();

            return _mapper.Map<List<ProductGetDTO>>(products);

        }

        public async Task<List<ProductGetDTO>> ProductPagination(int pageNumber = 1, int size = 10)
        {
            try
            {
                var products = await _context.Products
                    .Include(x => x.Category)
                    .Skip((pageNumber - 1) * size)
                    .Take(size)
                    .ToListAsync();

                return _mapper.Map<List<ProductGetDTO>>(products);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

}
