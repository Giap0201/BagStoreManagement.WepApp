using BagStore.Web.Models.ViewModels;
using BagStore.Web.Services;
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

        // 🟩 POST: /api/auth/register
        //[HttpPost("register")]
        //public async Task<IActionResult> Register([FromBody] RegisterModel model)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    var result = await _userService.RegisterAsync(model);

        //    if (result.Succeeded)
        //        return Ok(new { message = "Đăng ký thành công!" });

        //    return BadRequest(result.Errors);
        //}

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _userService.RegisterAsync(model);

                if (result.Succeeded)
                    return Ok(new { message = "Đăng ký thành công!" });

                return BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        // 🟦 POST: /api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.LoginAsync(model);

            //if (result.Succeeded)
            //    return Ok(new { message = "Đăng nhập thành công!" })

            //return Unauthorized(new { message = "Tên đăng nhập hoặc mật khẩu không đúng" });

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

            //// Với API login, ta trả JWT
            //var token = await _userService.GenerateJwtForUserAsync(model.UserName, model.Password);
            //if (token == null)
            //    return Unauthorized(new { message = "Tên đăng nhập hoặc mật khẩu không đúng" });

            //// 🔹 Trả về token cho client (Postman hoặc AJAX)
            //return Ok(new
            //{
            //    message = "Đăng nhập thành công!",
            //    token,
            //    token_type = "Bearer"
            //});
        }

        // 🟨 GET: /api/auth/profile/{userId}
        [HttpGet("profile/{userId}")]
        public async Task<IActionResult> GetProfile(string userId)
        {
            var user = await _userService.GetProfileAsync(userId);
            if (user == null)
                return NotFound(new { message = "Không tìm thấy người dùng" });

            return Ok(new
            {
                user.UserName,
                user.Email,
                user.FullName,
                user.NgaySinh
            });
        }

        //[HttpPut("profile")]
        //[Authorize]  // yêu cầu login
        //public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileModel model)
        //{
        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // lấy userId từ token
        //    var user = await _userService.GetProfileAsync(userId);

        //    if (user == null) return NotFound(new { message = "User không tồn tại" });

        //    // Cập nhật thông tin
        //    user.FullName = model.FullName;
        //    user.NgaySinh = model.NgaySinh;

        //    var result = await _userService.UpdateProfileAsync(user); // bạn cần thêm method này trong IUserService

        //    if (!result.Succeeded) return BadRequest(result.Errors);

        //    return Ok(new { message = "Cập nhật profile thành công!" });
        //}
    }
}
