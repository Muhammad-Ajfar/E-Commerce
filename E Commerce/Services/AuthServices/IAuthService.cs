using E_Commerce.DTOs;
using E_Commerce.Helpers;

namespace E_Commerce.Services.AuthServices
{
     public interface IAuthService
    {
        Task<ServiceResponse<UserResponseDTO>> RegisterUserAsync(RegisterUserDTO registerUserDTO);
        Task<ServiceResponse<string>> LoginUserAsync(LoginUserDTO loginUserDTO);
    }
}
