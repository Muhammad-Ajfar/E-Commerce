using AutoMapper;
using E_Commerce.Data;
using E_Commerce.DTOs;
using E_Commerce.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Services.CategoryServices
{

    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CategoryService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CategoryDto>> GetCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            return _mapper.Map<List<CategoryDto>>(categories);
        }

        public async Task<bool> AddCategory(CategoryDto categoryDto)
        {
            var isExist = await _context.Categories.AnyAsync(x => x.Name.ToLower() == categoryDto.Name.ToLower());
            if (!isExist)
            {
                var cat = _mapper.Map<Category>(categoryDto);
                await _context.Categories.AddAsync(cat);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> RemoveCategory(int id)
        {
            var res = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (res == null)
            {
                return false;
            }
            else
            {
                _context.Categories.Remove(res);
                await _context.SaveChangesAsync();
                return true;
            }
        }

    }
}
