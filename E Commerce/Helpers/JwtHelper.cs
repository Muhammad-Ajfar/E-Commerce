﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using E_Commerce.Models;
using Microsoft.IdentityModel.Tokens;

namespace E_Commerce.Helpers
{
    public interface IJwtHelper
    {
        string GenerateToken(User user);
    }

    public class JwtHelper : IJwtHelper
    {
        private readonly string _key;

        public JwtHelper(IConfiguration configuration)
        {
            _key = configuration["Jwt:Key"]; // Retrieve the key from appsettings.json
        }

        public string GenerateToken(User user)
        {
            // Convert the key to a symmetric security key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Define claims for the token
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Name, user.Name)
            };

            // Create the token
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(3), // Token expiration set to 1 hour
                signingCredentials: credentials);

            // Return the token as a string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
