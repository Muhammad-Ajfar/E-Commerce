using E_Commerce.DTOs;
using E_Commerce.Helpers;

namespace E_Commerce.Services.AddressServices
{
    public interface IAddressService
    {
        Task<ApiResponses<IEnumerable<AddressDTO>>> GetAddressesByUserId(Guid userId);
        Task<ApiResponses<AddressDTO>> AddAddress(AddressCreateDTO addressCreateDTO, Guid userId);
        Task<ApiResponses<bool>> RemoveAddress(Guid addressId, Guid userId);
    }

}
