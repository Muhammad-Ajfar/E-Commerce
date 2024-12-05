using AutoMapper;
using E_Commerce.Data;
using E_Commerce.DTOs;
using E_Commerce.Helpers;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Services
{
    public interface IUserService
    {
        Task<List<UserViewDTO>> GetUsers();
        Task<BlockUnblockResponse> BlockAndUnblock(Guid id);
        Task<UserViewDTO> GetUserById(Guid id);

    }

    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UserService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<UserViewDTO>> GetUsers()
        {
            try
            {
                var u = await _context.Users.ToListAsync();
                return _mapper.Map<List<UserViewDTO>>(u);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<UserViewDTO> GetUserById(Guid id)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
                if (user == null)
                {
                    return null;
                }
                return _mapper.Map<UserViewDTO>(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BlockUnblockResponse> BlockAndUnblock(Guid id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null) { throw new Exception("User not found"); }
                user.IsBlocked = !user.IsBlocked;
                await _context.SaveChangesAsync();

                return new BlockUnblockResponse
                {
                    IsBlocked = user.IsBlocked,
                    Message = user.IsBlocked ? "User is blocked" : "User is unblocked"
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
