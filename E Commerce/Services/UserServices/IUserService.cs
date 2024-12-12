using E_Commerce.DTOs;
using E_Commerce.Helpers;

namespace E_Commerce.Services.UserServices
{
    public interface IUserService
    {
        Task<List<UserViewDTO>> GetUsers();
        Task<BlockUnblockResponse> BlockAndUnblock(Guid id);
        Task<UserViewDTO> GetUserById(Guid id);

    }
}
