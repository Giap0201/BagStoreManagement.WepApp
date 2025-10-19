using BagStore.Models.Common;
using BagStore.Services.Interfaces;
using BagStore.Web.Models.Common;
using BagStore.Web.Models.DTOs;
using BagStore.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BagStore.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [ValidateModel] // Tự động bắt lỗi DataAnnotation và trả BaseResponse
    public class KichThuocApiController : ControllerBase
    {
        private readonly IKichThuocService _service;

        public KichThuocApiController(IKichThuocService service)
        {
            _service = service;
        }

        // GET: /api/KichThuoc
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _service.GetAllAsync();
            return Ok(response); // BaseResponse<List<KichThuocDto>>
        }

        // GET: /api/KichThuoc/{maKichThuoc}
        [HttpGet("{maKichThuoc}")]
        public async Task<IActionResult> GetById(int maKichThuoc)
        {
            var response = await _service.GetByIdAsync(maKichThuoc);
            return response.Status == "error" ? BadRequest(response) : Ok(response);
        }

        // POST: /api/KichThuoc
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] KichThuocDto dto)
        {
            var response = await _service.CreateAsync(dto);
            return response.Status == "error" ? BadRequest(response) : Ok(response);
        }

        // PUT: /api/KichThuoc/{maKichThuoc}
        [HttpPut("{maKichThuoc}")]
        public async Task<IActionResult> Update(int maKichThuoc, [FromBody] KichThuocDto dto)
        {
            if (maKichThuoc != dto.MaKichThuoc)
            {
                return BadRequest(BaseResponse<KichThuocDto>.Error(
                    new List<ErrorDetail> { new ErrorDetail("MaKichThuoc", "ID không khớp") },
                    "Cập nhật thất bại"));
            }

            var response = await _service.UpdateAsync(maKichThuoc, dto);
            return response.Status == "error" ? BadRequest(response) : Ok(response);
        }

        // DELETE: /api/KichThuoc/{maKichThuoc}
        [HttpDelete("{maKichThuoc}")]
        public async Task<IActionResult> Delete(int maKichThuoc)
        {
            var response = await _service.DeleteAsync(maKichThuoc);
            return response.Status == "error" ? BadRequest(response) : Ok(response);
        }
    }
}