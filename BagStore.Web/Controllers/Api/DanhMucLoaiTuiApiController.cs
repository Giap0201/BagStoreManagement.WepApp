using BagStore.Web.Models.DTOs;
using BagStore.Web.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BagStore.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class DanhMucLoaiTuiApiController : ControllerBase
    {
        private readonly IDanhMucLoaiTuiService _service;

        public DanhMucLoaiTuiApiController(IDanhMucLoaiTuiService service)
        {
            _service = service;
        }

        // GET: api/DanhMucLoaiTuiApi
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
                return StatusCode(500, new { message = "Lỗi server khi lấy danh sách loại túi.", Details = ex.Message });
            }
        }

        // GET: api/DanhMucLoaiTuiApi/1
        [HttpGet("{maLoaiTui}")]
        public async Task<IActionResult> GetById(int maLoaiTui)
        {
            try
            {
                var dto = await _service.GetByIdAsync(maLoaiTui);
                if (dto == null)
                    return NotFound(new { message = $"Không tìm thấy Loại Túi với mã: {maLoaiTui}" });

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

        // POST: api/DanhMucLoaiTuiApi
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DanhMucLoaiTuiDto dto)
        {
            if (dto == null)
                return BadRequest(new { message = "Dữ liệu không hợp lệ" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var created = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { maLoaiTui = created.MaLoaiTui }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi tạo loại túi.", Details = ex.Message });
            }
        }

        // PUT: api/DanhMucLoaiTuiApi/1
        [HttpPut("{maLoaiTui}")]
        public async Task<IActionResult> Update(int maLoaiTui, [FromBody] DanhMucLoaiTuiDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (maLoaiTui != dto.MaLoaiTui)
                return BadRequest(new { message = "Mã loại túi trong URL và Body không khớp." });

            try
            {
                var updated = await _service.UpdateAsync(maLoaiTui, dto);
                if (updated == null)
                    return NotFound(new { message = $"Không tìm thấy Loại Túi với mã: {maLoaiTui} để cập nhật." });

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

        // DELETE: api/DanhMucLoaiTuiApi/1
        [HttpDelete("{maLoaiTui}")]
        public async Task<IActionResult> Delete(int maLoaiTui)
        {
            try
            {
                var success = await _service.DeleteAsync(maLoaiTui);
                if (!success)
                    return NotFound(new { message = $"Không tìm thấy Loại Túi với mã: {maLoaiTui} để xóa." });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server: " + ex.Message });
            }
        }
    }
}