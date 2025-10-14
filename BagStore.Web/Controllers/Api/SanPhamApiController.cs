using BagStore.Web.Models.DTOs.SanPhams;
using BagStore.Web.Models.ViewModels.SanPhams;
using BagStore.Web.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace BagStore.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SanPhamApiController : ControllerBase
    {
        private readonly ISanPhamService _service;

        public SanPhamApiController(ISanPhamService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] SanPhamCreateDto dto)
        {
            try
            {
                var result = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetByIdAsync), new { maSP = result.MaSP }, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Đã xảy ra lỗi khi thêm sản phẩm.", Details = ex.Message });
            }
        }

        [HttpGet("{maSP}")]
        public async Task<IActionResult> GetByIdAsync(int maSP)
        {
            try
            {
                var result = await _service.GetByIdAsync(maSP);
                if (result == null)
                    return NotFound(new { Message = $"Không tìm thấy sản phẩm với id = {maSP}" });

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

        [HttpPut("{maSP}")]
        public async Task<IActionResult> Update(int maSP, [FromBody] SanPhamUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var result = await _service.UpdateAsync(maSP, dto);
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

        [HttpDelete("{maSP}")]
        public async Task<IActionResult> Delete(int maSP)
        {
            try
            {
                var success = await _service.DeleteAsync(maSP);
                if (!success)
                    return NotFound(new { message = "Sản phẩm không tồn tại" });
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server: " + ex.Message });
            }
        }
    }
}