using BagStore.Web.Models.DTOs;
using BagStore.Web.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BagStore.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThuongHieuApiController : ControllerBase
    {
        private readonly IThuongHieuService _service;

        public ThuongHieuApiController(IThuongHieuService service)
        {
            _service = service;
        }

        // GET: api/ThuongHieuApi
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
                return StatusCode(500, new { message = "Lỗi server khi lấy danh sách thương hiệu.", Details = ex.Message });
            }
        }

        // GET: api/ThuongHieuApi/1
        [HttpGet("{maThuongHieu}")]
        public async Task<IActionResult> GetById(int maThuongHieu)
        {
            try
            {
                var dto = await _service.GetByIdAsync(maThuongHieu);
                if (dto == null)
                    return NotFound(new { message = $"Không tìm thấy Thương hiệu với mã: {maThuongHieu}" });

                return Ok(dto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server: " + ex.Message });
            }
        }

        // POST: api/ThuongHieuApi
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ThuongHieuDto dto)
        {
            if (dto == null)
                return BadRequest(new { message = "Dữ liệu không hợp lệ" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var created = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { maThuongHieu = created.MaThuongHieu }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi tạo thương hiệu.", Details = ex.Message });
            }
        }

        // PUT: api/ThuongHieuApi/1
        [HttpPut("{maThuongHieu}")]
        public async Task<IActionResult> Update(int maThuongHieu, [FromBody] ThuongHieuDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (maThuongHieu != dto.MaThuongHieu)
                return BadRequest(new { message = "Mã thương hiệu trong URL và Body không khớp." });

            try
            {
                var updated = await _service.UpdateAsync(maThuongHieu, dto);
                if (updated == null)
                    return NotFound(new { message = $"Không tìm thấy Thương hiệu với mã: {maThuongHieu} để cập nhật." });

                return Ok(updated);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server: " + ex.Message });
            }
        }

        // DELETE: api/ThuongHieuApi/1
        [HttpDelete("{maThuongHieu}")]
        public async Task<IActionResult> Delete(int maThuongHieu)
        {
            try
            {
                var success = await _service.DeleteAsync(maThuongHieu);
                if (!success)
                    return NotFound(new { message = $"Không tìm thấy Thương hiệu với mã: {maThuongHieu} để xóa." });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server: " + ex.Message });
            }
        }
    }
}
