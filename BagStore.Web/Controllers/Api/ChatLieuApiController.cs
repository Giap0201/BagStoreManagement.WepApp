using BagStore.Models.Common;
using BagStore.Web.Models.Common;
using BagStore.Web.Models.DTOs;
using BagStore.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BagStore.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [ValidateModel] // Áp dụng ValidateModelAttribute để tự động bắt lỗi DataAnnotation
    [Authorize(Roles = "Admin")]
    public class ChatLieuApiController : ControllerBase
    {
        private readonly IChatLieuService _service;

        public ChatLieuApiController(IChatLieuService service)
        {
            _service = service;
        }

        // GET: /api/ChatLieu
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _service.GetAllAsync();
            return Ok(response); // BaseResponse<List<ChatLieuDto>>
        }

        // GET: /api/ChatLieu/{maChatLieu}
        [HttpGet("{maChatLieu}")]
        public async Task<IActionResult> GetById(int maChatLieu)
        {
            var response = await _service.GetByIdAsync(maChatLieu);
            return response.Status == "error" ? BadRequest(response) : Ok(response);
        }

        // POST: /api/ChatLieu
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ChatLieuDto dto)
        {
            var response = await _service.CreateAsync(dto);
            return response.Status == "error" ? BadRequest(response) : Ok(response);
        }

        // PUT: /api/ChatLieu/{maChatLieu}
        [HttpPut("{maChatLieu}")]
        public async Task<IActionResult> Update(int maChatLieu, [FromBody] ChatLieuDto dto)
        {
            if (maChatLieu != dto.MaChatLieu)
            {
                return BadRequest(BaseResponse<ChatLieuDto>.Error(
                    new List<ErrorDetail> { new ErrorDetail("MaChatLieu", "ID không khớp") },
                    "Cập nhật thất bại"));
            }

            var response = await _service.UpdateAsync(maChatLieu, dto);
            return response.Status == "error" ? BadRequest(response) : Ok(response);
        }

        // DELETE: /api/ChatLieu/{maChatLieu}
        [HttpDelete("{maChatLieu}")]
        public async Task<IActionResult> Delete(int maChatLieu)
        {
            var response = await _service.DeleteAsync(maChatLieu);
            return response.Status == "error" ? BadRequest(response) : Ok(response);
        }
    }
}