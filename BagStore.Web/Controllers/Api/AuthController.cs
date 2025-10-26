using BagStore.Web.Models.ViewModels;
using BagStore.Web.Services;
using BagStore.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BagStore.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        // 🟦 POST: /api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.LoginAsync(model);

            if (!result.Succeeded)
                return Unauthorized(new { message = "Tên đăng nhập hoặc mật khẩu không đúng" });

            // ✅ Tạo JWT token
            var user = await _userService.GetProfileByUserNameAsync(model.UserName);
            if (user == null)
                return Unauthorized(new { message = "Không tìm thấy người dùng" });

            var token = _userService.GenerateJwtToken(user);

            // ✅ Trả về token cho client
            return Ok(new
            {
                message = "Đăng nhập thành công!",
                token
            });
        }
    }
}
