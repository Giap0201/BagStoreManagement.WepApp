using BagStore.Models.Common;
using BagStore.Services.Interfaces;
using BagStore.Web.Models.Common;
using BagStore.Web.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BagStore.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [ValidateModel] // Tự động bắt lỗi DataAnnotation và trả BaseResponse
    public class DanhMucLoaiTuiApiController : ControllerBase
    {
        private readonly IDanhMucLoaiTuiService _service;

        public DanhMucLoaiTuiApiController(IDanhMucLoaiTuiService service)
        {
            _service = service;
        }

        // GET: /api/DanhMucLoaiTui
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _service.GetAllAsync();
            return Ok(response);
        }

        // GET: /api/DanhMucLoaiTui/{maLoaiTui}
        [HttpGet("{maLoaiTui}")]
        public async Task<IActionResult> GetById(int maLoaiTui)
        {
            var response = await _service.GetByIdAsync(maLoaiTui);
            return response.Status == "error" ? BadRequest(response) : Ok(response);
        }

        // POST: /api/DanhMucLoaiTui
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DanhMucLoaiTuiDto dto)
        {
            var response = await _service.CreateAsync(dto);
            return response.Status == "error" ? BadRequest(response) : Ok(response);
        }

        // PUT: /api/DanhMucLoaiTui/{maLoaiTui}
        [HttpPut("{maLoaiTui}")]
        public async Task<IActionResult> Update(int maLoaiTui, [FromBody] DanhMucLoaiTuiDto dto)
        {
            if (maLoaiTui != dto.MaLoaiTui)
            {
                return BadRequest(BaseResponse<DanhMucLoaiTuiDto>.Error(
                    new List<ErrorDetail> { new ErrorDetail("MaLoaiTui", "ID không khớp") },
                    "Cập nhật thất bại"));
            }

            var response = await _service.UpdateAsync(maLoaiTui, dto);
            return response.Status == "error" ? BadRequest(response) : Ok(response);
        }

        // DELETE: /api/DanhMucLoaiTui/{maLoaiTui}
        [HttpDelete("{maLoaiTui}")]
        public async Task<IActionResult> Delete(int maLoaiTui)
        {
            var response = await _service.DeleteAsync(maLoaiTui);
            return response.Status == "error" ? BadRequest(response) : Ok(response);
        }
    }
}