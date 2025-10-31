using BagStore.Models.Common;
using BagStore.Web.Models.Common;
using BagStore.Web.Models.DTOs.SanPhams;
using BagStore.Web.Models.ViewModels.SanPhams;
using BagStore.Web.Services.Implementations;
using BagStore.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BagStore.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [ValidateModel] // Tự động bắt lỗi DataAnnotation và trả BaseResponse
    public class SanPhamApiController : ControllerBase
    {
        private readonly ISanPhamService _service;

        //Dependency Injection
        public SanPhamApiController(ISanPhamService service)
        {
            _service = service;
        }

        // GET: /api/SanPhamApi
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _service.GetAllAsync();
            return Ok(response);
        }
        [HttpGet("GetAllPaged")]
        public async Task<IActionResult> GetAllPaged(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 6,
            [FromQuery] string? keyword = null,
            [FromQuery] int? maLoaiTui = null,
            [FromQuery] int? maThuongHieu = null,
            [FromQuery] int? maChatLieu = null)
        {
            var response = await _service.GetAllPagedAsync(page, pageSize, keyword, maLoaiTui, maThuongHieu, maChatLieu);

            // response expected: BaseResponse<PagedResult<SanPhamResponseDto>>
            if (response == null) return StatusCode(500, BaseResponse<object>.Error("Lỗi server"));
            return Ok(response);
        }



        // GET: /api/SanPhamApi/{maLoaiTui}
        [HttpGet("{maSanPham}")]
        public async Task<IActionResult> GetById(int maSanPham)
        {
            var response = await _service.GetByIdAsync(maSanPham);
            return response.Status == "error" ? BadRequest(response) : Ok(response);
        }

        // POST: /api/SanPhamApi
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] SanPhamCreateDto dto)
        {
            var response = await _service.CreateAsync(dto);
            return response.Status == "error" ? BadRequest(response) : Ok(response);
        }

        // PUT: /api/SanPhamApi/{maSanPham}

        [HttpPut("{maSanPham}")]
        public async Task<IActionResult> Update(int maSanPham, [FromBody] SanPhamUpdateDto dto)
        {
            if (maSanPham != dto.MaSanPham)
            {
                return BadRequest(BaseResponse<SanPhamResponseDto>.Error(
                   new List<ErrorDetail> { new ErrorDetail("MaSanPham", "ID không khớp") },
                   "Cập nhật thất bại"));
            }
            var response = await _service.UpdateAsync(maSanPham, dto);
            return response.Status == "error" ? BadRequest(response) : Ok(response);
        }

        // DELETE: /api/SanPhamApi/{maSanPham}
        [HttpDelete("{maSanPham}")]
        public async Task<IActionResult> Delete(int maSanPham)
        {
            var response = await _service.DeleteAsync(maSanPham);
            return response.Status == "error" ? BadRequest(response) : Ok(response);
        }
    }
}