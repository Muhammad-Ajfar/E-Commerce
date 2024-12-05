using E_Commerce.DTOs;
using E_Commerce.Models;
using E_Commerce.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _services;
        public ProductController(IProductService services)
        {
            _services = services;
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _services.GetProducts();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
        
            var p = await _services.GetProductById(id);
            return Ok(p);

        }
        [HttpGet("getByCategory")]
        public async Task<IActionResult> GetByCategory(string categoryName)
        {
            try
            {
                var p = await _services.GetProductByCategory(categoryName);
                return Ok(p);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            try
            {
                bool res = await _services.DeleteProduct(id);
                if (res)
                {
                    return Ok("Product deleted succesfully");
                }
                return NotFound("No product found");

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost("Add")]
        public async Task<IActionResult> AddProduct([FromForm] ProductDTO productDto, IFormFile image)
        {

            try
            {
                await _services.AddProduct(productDto, image);
                return Ok("Product Succesfully added");
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("UpdateProduct/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromForm] ProductDTO productDto, IFormFile image)
        {
            try
            {
                await _services.UpdateProduct(id, productDto, image);
                return Ok("Updated seccefully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("search-item")]
        public async Task<IActionResult> SearchProduct(string search)
        {
            try
            {
                var res = await _services.SearchProduct(search);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("paginated-products")]
        public async Task<IActionResult> pagination([FromQuery] int pageNumber = 1, [FromQuery] int size = 10)
        {
            try
            {
                var res = await _services.ProductPagination(pageNumber, size);
                return Ok(res);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
