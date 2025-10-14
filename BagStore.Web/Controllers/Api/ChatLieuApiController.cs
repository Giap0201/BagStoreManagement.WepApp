using BagStore.Web.Models.DTOs;
using BagStore.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BagStore.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatLieuApiController : ControllerBase
    {
        private readonly IChatLieuService _service;

        public ChatLieuApiController(IChatLieuService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var dtos = await _service.GetAllAsync();
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server khi lấy danh sách chất liệu.", Details = ex.Message });
            }
        }

        [HttpGet("{maChatLieu}")]
        public async Task<IActionResult> GetById(int maChatLieu)
        {
            try
            {
                var dto = await _service.GetByIdAsync(maChatLieu);
                return Ok(dto);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ChatLieuDto dto)
        {
            if (dto == null) return BadRequest(new { message = "Dữ liệu không hợp lệ" });
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var created = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { maChatLieu = created.MaChatLieu }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server khi tạo chất liệu.", Details = ex.Message });
            }
        }

        [HttpPut("{maChatLieu}")]
        public async Task<IActionResult> Update(int maChatLieu, [FromBody] ChatLieuDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (maChatLieu != dto.MaChatLieu) return BadRequest(new { message = "Mã chất liệu trong URL và Body không khớp." });

            try
            {
                var updated = await _service.UpdateAsync(maChatLieu, dto);
                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server: " + ex.Message });
            }
        }

        [HttpDelete("{maChatLieu}")]
        public async Task<IActionResult> Delete(int maChatLieu)
        {
            try
            {
                var success = await _service.DeleteAsync(maChatLieu);
                if (!success) return NotFound(new { message = $"Không tìm thấy chất liệu với mã: {maChatLieu}" });

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server: " + ex.Message });
            }
        }
    }
}