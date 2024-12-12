using AutoMapper;
using BCrypt.Net;
using E_Commerce.Data;
using E_Commerce.DTOs;
using E_Commerce.Helpers;
using E_Commerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Services.AuthServices
{

    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IJwtHelper _jwtHelper;
        private readonly IMapper _mapper;
        public AuthService(AppDbContext context, IJwtHelper jwtHelper, IMapper mapper)
        {
            _context = context;
            _jwtHelper = jwtHelper;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<UserResponseDTO>> RegisterUserAsync(RegisterUserDTO registerUserDTO)
        {
            // Check if email is already registered
            if (await _context.Users.AnyAsync(u => u.Email == registerUserDTO.Email))
            {
                return new ServiceResponse<UserResponseDTO>
                {
                    IsSuccess = false,
                    Message = "Email is already registered."
                };
            }

            // Map DTO to User model
            var user = _mapper.Map<User>(registerUserDTO);

            // Hash password
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerUserDTO.Password);

            // Save user
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Map user to response DTO
            var savedUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == registerUserDTO.Email);
            var userResponse = _mapper.Map<UserResponseDTO>(savedUser);

            return new ServiceResponse<UserResponseDTO>
            {
                IsSuccess = true,
                Data = userResponse,
                Message = "User registered successfully."
            };
        }

        public async Task<ServiceResponse<string>> LoginUserAsync(LoginUserDTO loginUserDTO)
        {
            // Check if user exists
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == loginUserDTO.Email);
            if (user == null)
            {
                return new ServiceResponse<string>
                {
                    IsSuccess = false,
                    Message = "Invalid email or password."
                };
            }

            // Verify password
            var pass = BCrypt.Net.BCrypt.Verify(loginUserDTO.Password, user.PasswordHash);
            if (!pass)
            {
                return new ServiceResponse<string>
                {
                    IsSuccess = false,
                    Message = "Invalid email or password."
                };
            }

            //Checking Blocked or not
            if (user.IsBlocked)
            {
                return new ServiceResponse<string>
                {
                    IsSuccess = false,
                    Message = "User is Blocked"
                };
            }

            // Generate JWT token
            var token = _jwtHelper.GenerateToken(user);

            return new ServiceResponse<string>
            {
                IsSuccess = true,
                Data = token,
                Message = "Login successful."
            };
        }
    }
}
