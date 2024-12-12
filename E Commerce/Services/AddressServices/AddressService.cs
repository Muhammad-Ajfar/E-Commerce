using AutoMapper;
using E_Commerce.Data;
using E_Commerce.DTOs;
using E_Commerce.Helpers;
using E_Commerce.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Services.AddressServices
{
    public class AddressService : IAddressService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AddressService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // 1. Get all addresses for a user
        public async Task<ApiResponses<IEnumerable<AddressDTO>>> GetAddressesByUserId(Guid userId)
        {
            var addresses = await _context.Addresses
                .Where(a => a.UserId == userId)
                .ToListAsync();

            if (addresses == null || !addresses.Any())
            {
                return new ApiResponses<IEnumerable<AddressDTO>>(404, "No addresses found for this user.");
            }

            var addressesDTO = _mapper.Map<IEnumerable<AddressDTO>>(addresses);
            return new ApiResponses<IEnumerable<AddressDTO>>(200, "Addresses retrieved successfully.", addressesDTO);
        }

        // 2. Add a new address
        public async Task<ApiResponses<AddressDTO>> AddAddress(AddressCreateDTO addressCreateDTO, Guid userId)
        {
            var newAddress = _mapper.Map<Address>(addressCreateDTO);
            newAddress.UserId = userId; // Assign the current user ID

            await _context.Addresses.AddAsync(newAddress);
            await _context.SaveChangesAsync();

            var addressDTO = _mapper.Map<AddressDTO>(newAddress);
            return new ApiResponses<AddressDTO>(201, "Address added successfully.", addressDTO);
        }

        // 3. Remove an address
        public async Task<ApiResponses<bool>> RemoveAddress(Guid addressId, Guid userId)
        {
            var address = await _context.Addresses
                .FirstOrDefaultAsync(a => a.Id == addressId && a.UserId == userId);

            if (address == null)
            {
                return new ApiResponses<bool>(404, "Address not found for this user.");
            }

            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();

            return new ApiResponses<bool>(200, "Address removed successfully.", true);
        }
    }

}
