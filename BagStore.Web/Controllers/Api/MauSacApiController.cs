using BagStore.Web.Models.DTOs;
using BagStore.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BagStore.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class MauSacApiController : ControllerBase
    {
        private readonly IMauSacService _service;

        public MauSacApiController(IMauSacService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = await _service.GetAllAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server khi lấy danh sách màu sắc", Details = ex.Message });
            }
        }

        [HttpGet("{maMauSac}")]
        public async Task<IActionResult> GetById(int maMauSac)
        {
            try
            {
                var dto = await _service.GetByIdAsync(maMauSac);
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
        public async Task<IActionResult> Create([FromBody] MauSacDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var created = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { maMauSac = created.MaMauSac }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server khi tạo màu sắc", Details = ex.Message });
            }
        }

        [HttpPut("{maMauSac}")]
        public async Task<IActionResult> Update(int maMauSac, [FromBody] MauSacDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (maMauSac != dto.MaMauSac)
                return BadRequest(new { message = "Mã màu trong URL và Body không khớp" });

            try
            {
                var updated = await _service.UpdateAsync(maMauSac, dto);
                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server khi cập nhật màu sắc", Details = ex.Message });
            }
        }

        [HttpDelete("{maMauSac}")]
        public async Task<IActionResult> Delete(int maMauSac)
        {
            try
            {
                var success = await _service.DeleteAsync(maMauSac);
                if (!success)
                    return NotFound(new { message = $"Không tìm thấy màu sắc với mã {maMauSac}" });
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server khi xóa màu sắc", Details = ex.Message });
            }
        }
    }
}