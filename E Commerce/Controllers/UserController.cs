using E_Commerce.DTOs;
using E_Commerce.Helpers;
using E_Commerce.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userServices;
        public UserController(IUserService userServices)
        {
            _userServices = userServices;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("getusers")]

        public async Task<IActionResult> getUsers()
        {
            try
            {
                var users = await _userServices.GetUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUser(Guid userId)
        {
            try
            {
                var user = await _userServices.GetUserById(userId);

                if (user == null)
                    return NotFound(new ApiResponses<string>(404, "User not found", null));

                var res = new ApiResponses<UserViewDTO>(200, "Fetched user by id", user);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500, "An error occurred", null, ex.Message));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("blockorUnblock/{userId}")]
        public async Task<IActionResult> BlockOrUnblockUser(Guid userId)
        {
            try
            {
                var result = await _userServices.BlockAndUnblock(userId);
                var response = new ApiResponses<BlockUnblockResponse>(200, "Block/unblock action performed", result);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500, "An error occurred", null, ex.Message));
            }
        }
    }
}
