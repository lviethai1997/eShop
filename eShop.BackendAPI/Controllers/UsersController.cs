using eShop.Application.System.Users;
using eShop.ViewModels.System.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace eShop.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authencicate([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultToken = await _userService.Authencate(request);
            if (string.IsNullOrEmpty(resultToken.ResultObject))
            {
                return BadRequest("Username or Password is Incorrect");
            }

            return Ok(resultToken);
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var Register = await _userService.Register(request);
            if (!Register.IsSuccessed)
            {
                return BadRequest(Register.Message);
            }
            return Ok(Register);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPagingUser([FromQuery] UserPagingRequest request)
        {
            var users = await _userService.GetUserPaging(request);
            return Ok(users);
        }

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromQuery] Guid id, [FromBody] UserUpdateRequest userUpdateRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var UserUpdate = await _userService.UpdateUser(id, userUpdateRequest);
            if (!UserUpdate.IsSuccessed)
            {
                return BadRequest(UserUpdate);
            }
            else
            {
                return Ok(UserUpdate);
            }
        }

        [HttpGet("GetUserById")]
        public async Task<IActionResult> GetUserById([FromQuery] string id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null) { return BadRequest(); }
            return Ok(user);
        }

        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser([FromQuery] string id)
        {
            var deleteUser = await _userService.DeleteUser(id);
            if (!deleteUser.IsSuccessed) { return BadRequest(); }
            return Ok(deleteUser);
        }
    }
}