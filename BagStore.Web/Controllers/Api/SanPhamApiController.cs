using BagStore.Web.Models.DTOs.SanPhams;
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
        public async Task<IActionResult> CreateSanPham([FromForm] SanPhamCreateDto dto)
        {
            try
            {
                var result = await _service.CreateSanPhamAsync(dto);
                return CreatedAtAction(nameof(GetSanPhamById), new { id = result.MaSP }, result);
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

        [HttpGet("{maSanPham}")]
        public async Task<IActionResult> GetSanPhamById(int maSanPham)
        {
            var result = await _service.GetSanPhamByIdAsync(maSanPham);
            if (result == null)
                return NotFound(new { Message = $"Không tìm thấy sản phẩm với id = {maSanPham}" });

            return Ok(result);
        }
    }
}