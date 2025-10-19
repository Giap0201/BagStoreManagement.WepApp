using BagStore.Models.Common;
using BagStore.Web.Models.Common;
using BagStore.Web.Models.DTOs;
using BagStore.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BagStore.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [ValidateModel] // Áp dụng ValidateModelAttribute để tự động bắt lỗi DataAnnotation
    public class ThuongHieuApiController : ControllerBase
    {
        private readonly IThuongHieuService _service;

        public ThuongHieuApiController(IThuongHieuService service)
        {
            _service = service;
        }

        // GET: /api/ThuongHieu
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _service.GetAllAsync();
            return Ok(response); // BaseResponse<List<ThuongHieuDto>>
        }

        // GET: /api/ThuongHieu/{maThuongHieu}
        [HttpGet("{maThuongHieu}")]
        public async Task<IActionResult> GetById(int maThuongHieu)
        {
            var response = await _service.GetByIdAsync(maThuongHieu);
            return response.Status == "error" ? BadRequest(response) : Ok(response);
        }

        // POST: /api/ThuongHieu
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ThuongHieuDto dto)
        {
            var response = await _service.CreateAsync(dto);
            return response.Status == "error" ? BadRequest(response) : Ok(response);
        }

        // PUT: /api/ThuongHieu/{maThuongHieu}
        [HttpPut("{maThuongHieu}")]
        public async Task<IActionResult> Update(int maThuongHieu, [FromBody] ThuongHieuDto dto)
        {
            if (maThuongHieu != dto.MaThuongHieu)
            {
                return BadRequest(BaseResponse<ThuongHieuDto>.Error(
                    new List<ErrorDetail> { new ErrorDetail("MaThuongHieu", "ID không khớp") },
                    "Cập nhật thất bại"));
            }

            var response = await _service.UpdateAsync(maThuongHieu, dto);
            return response.Status == "error" ? BadRequest(response) : Ok(response);
        }

        // DELETE: /api/ThuongHieu/{maThuongHieu}
        [HttpDelete("{maThuongHieu}")]
        public async Task<IActionResult> Delete(int maThuongHieu)
        {
            var response = await _service.DeleteAsync(maThuongHieu);
            return response.Status == "error" ? BadRequest(response) : Ok(response);
        }
    }
}