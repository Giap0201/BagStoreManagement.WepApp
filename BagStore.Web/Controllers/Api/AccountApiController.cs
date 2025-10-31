using BagStore.Web.Models.Entities;
using BagStore.Web.Models.ViewModels;
using BagStore.Web.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BagStore.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AccountApiController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountApiController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.RegisterCustomerAsync(model);

            if (!result.Succeeded)
                return BadRequest(result.Errors.Select(e => e.Description));

            return Ok(new { message = "Đăng ký tài khoản thành công" });
        }

        // GET api/AccountApi/profile
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            //var userId = User.FindFirst("uid")?.Value; // Lấy UserId từ JWT
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var user = await _userService.GetProfileAsync(userId);
            if (user == null) return NotFound();

            return Ok(new
            {
                id = user.Id,
                userName = user.UserName,
                fullName = user.FullName,
                email = user.Email,
                phoneNumber = user.PhoneNumber,
                ngaySinh = user.NgaySinh
            });
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] ProfileEditViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            model.Id = userId; // gán để UpdateProfileAsync dùng model.Id

            var result = await _userService.UpdateProfileAsync(model);
            if (!result.Succeeded)
                return BadRequest(result.Errors.Select(e => e.Description));

            return Ok(new { message = "Cập nhật hồ sơ thành công" });
        }

        // DELETE api/AccountApi/delete
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAccount()
        {
            //var userId = User.FindFirst("uid")?.Value;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var result = await _userService.DeleteAccountAsync(userId);
            if (!result.Succeeded)
                return BadRequest(result.Errors.Select(e => e.Description));

            return Ok(new { message = "Tài khoản đã bị xoá" });
        }

        [Authorize]
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _userService.ChangePasswordAsync(userId, model.OldPassword, model.NewPassword);

            if (!result.Succeeded)
                return BadRequest(result.Errors.Select(e => e.Description));

            return Ok(new { message = "Đổi mật khẩu thành công" });
        }
    }
}
