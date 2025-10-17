using BagStore.Web.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BagStore.Web.Models.Entities;

namespace BagStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
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
            var list = await _userService.GetAllCustomersAsync();
            // map to DTO if cần
            return Ok(list.Select(u => new {
                u.Id,
                u.UserName,
                u.FullName,
                u.Email,
                u.PhoneNumber,
                NgaySinh = u.NgaySinh?.ToString("yyyy-MM-dd")
            }));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var res = await _userService.DeleteAccountAsync(id);
            if (res.Succeeded) return Ok();
            return BadRequest(res.Errors);
        }
    }
}
