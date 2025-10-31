using BagStore.Web.Models.DTOs;
using BagStore.Web.Models.DTOs.SanPhams;
using BagStore.Web.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace BagStore.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChiTietSanPhamApiController : ControllerBase
    {
        private readonly IChiTietSanPhamService _service;

        public ChiTietSanPhamApiController(IChiTietSanPhamService service)
        {
            _service = service;
        }

        // phuong thuc tao moi chi tiet san pham
        //https://localhost:7013/api/ChiTietSanPhamApi
        [HttpPost("{maSP}")]
        public async Task<IActionResult> Add(int maSP, [FromBody] ChiTietSanPhamCreateDto dto)
        {
            if (dto == null)
                return BadRequest(new { message = "Dữ liệu không hợp lệ" });
            try
            {
                var result = await _service.AddAsync(maSP, dto);
                return CreatedAtAction(nameof(GetByIdAsync), new { maChiTietSanPham = result.MaChiTietSP }, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi thêm biến thể.", Details = ex.Message });
            }
        }

        //https://localhost:7013/api/ChiTietSanPhamApi/1

        [HttpGet("{maChiTietSanPham}")]
        public async Task<IActionResult> GetByIdAsync(int maChiTietSanPham)
        {
            try
            {
                var result = await _service.GetByIdAsync(maChiTietSanPham);
                if (result == null) return NotFound(new { Message = $"Không tìm thấy biến thể có id = {maChiTietSanPham}" });
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server : " + ex.Message });
            }
        }

        [HttpPut("{maChiTietSanPham}")]
        public async Task<IActionResult> Update(int maChiTietSanPham, [FromBody] ChiTietSanPhamCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest();
            try
            {
                var result = await _service.UpdateAsync(maChiTietSanPham, dto);
                return Ok(result);
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

        [HttpDelete("{maChiTietSanPham}")]
        public async Task<IActionResult> Delete(int maChiTietSanPham)
        {
            try
            {
                var success = await _service.DeleteAsync(maChiTietSanPham);
                if (!success)
                    return NotFound(new { message = "Biến thể không tồn tại" });
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server: " + ex.Message });
            }
        }
    }
}