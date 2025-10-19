using BagStore.Web.Models.Entities;
using BagStore.Web.Models.ViewModels;
using BagStore.Web.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BagStore.Web.Controllers.Api
{
    [Area("Admin")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
    [Authorize(Roles = "Admin")]
    public class CustomerApiController : ControllerBase
    {
        private readonly IUserService _userService;
        public CustomerApiController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllCustomersAsync();
            // map to simple DTO to avoid leaking sensitive fields
            var list = users.Select(u => new {
                id = u.Id,
                userName = u.UserName,
                fullName = u.FullName,
                email = u.Email,
                phoneNumber = u.PhoneNumber,
                ngaySinh = u.NgaySinh
            });
            return Ok(list);
        }

        // GET api/Admin/CustomerApi/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _userService.GetByIdAsync(id);
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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AdminCustomerViewModel model)
        {
            if (model == null)
                return BadRequest("Model không hợp lệ.");

            // Gán mật khẩu mặc định nếu chưa có
            if (string.IsNullOrEmpty(model.Password))
                model.Password = "Customer@123";

            var result = await _userService.CreateCustomerAsync(model);
            if (!result.Succeeded)
                return BadRequest(result.Errors.Select(e => e.Description));

            return Ok(new { message = "Tạo khách hàng thành công!" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] AdminCustomerViewModel model)
        {
            if (model == null)
                return BadRequest("Model không hợp lệ.");

            // ép Id từ URL vào model nếu cần
            model.Id = id;

            var result = await _userService.UpdateCustomerAsync(model);
            if (!result.Succeeded)
                return BadRequest(result.Errors.Select(e => e.Description));

            return Ok(new { message = "Cập nhật thành công!" });
        }


        // DELETE api/Admin/CustomerApi/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _userService.DeleteAccountAsync(id);
            if (!result.Succeeded)
                return BadRequest(result.Errors.Select(e => e.Description));
            return NoContent();
        }

        // POST api/Admin/CustomerApi/resetpassword
        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _userService.ResetPasswordAsync(model.Id, model.NewPassword);
            if (!result.Succeeded)
                return BadRequest(result.Errors.Select(e => e.Description));
            return Ok(new { message = "Đặt lại mật khẩu thành công" });
        }
    }
}
