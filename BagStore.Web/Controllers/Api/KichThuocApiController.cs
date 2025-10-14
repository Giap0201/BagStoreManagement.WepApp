using BagStore.Web.Models.DTOs;
using BagStore.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BagStore.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class KichThuocApiController : ControllerBase
    {
        private readonly IKichThuocService _service;
        public KichThuocApiController(IKichThuocService service) => _service = service;

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
                return StatusCode(500, new { message = "Lỗi server khi lấy danh sách kích thước", Details = ex.Message });
            }
        }

        [HttpGet("{maKichThuoc}")]
        public async Task<IActionResult> GetById(int maKichThuoc)
        {
            try
            {
                var dto = await _service.GetByIdAsync(maKichThuoc);
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
        public async Task<IActionResult> Create([FromBody] KichThuocDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var created = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { maKichThuoc = created.MaKichThuoc }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server khi tạo kích thước", Details = ex.Message });
            }
        }

        [HttpPut("{maKichThuoc}")]
        public async Task<IActionResult> Update(int maKichThuoc, [FromBody] KichThuocDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (maKichThuoc != dto.MaKichThuoc)
                return BadRequest(new { message = "Mã kích thước trong URL và Body không khớp" });

            try
            {
                var updated = await _service.UpdateAsync(maKichThuoc, dto);
                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server khi cập nhật kích thước", Details = ex.Message });
            }
        }

        [HttpDelete("{maKichThuoc}")]
        public async Task<IActionResult> Delete(int maKichThuoc)
        {
            try
            {
                var success = await _service.DeleteAsync(maKichThuoc);
                if (!success)
                    return NotFound(new { message = $"Không tìm thấy kích thước với mã {maKichThuoc}" });
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server khi xóa kích thước", Details = ex.Message });
            }
        }
    }
}
