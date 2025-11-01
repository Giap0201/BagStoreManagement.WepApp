using BagStore.Models.Common;
using BagStore.Web.Models.Common;
using BagStore.Web.Models.DTOs;
using BagStore.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BagStore.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [ValidateModel] // Tự động bắt lỗi DataAnnotation và trả BaseResponse
    [Authorize(Roles = "Admin")]
    public class MauSacApiController : ControllerBase
    {
        private readonly IMauSacService _service;

        public MauSacApiController(IMauSacService service)
        {
            _service = service;
        }

        // GET: /api/MauSac
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _service.GetAllAsync();
            return Ok(response); // BaseResponse<List<MauSacDto>>
        }

        // GET: /api/MauSac/{maMauSac}
        [HttpGet("{maMauSac}")]
        public async Task<IActionResult> GetById(int maMauSac)
        {
            var response = await _service.GetByIdAsync(maMauSac);
            return response.Status == "error" ? BadRequest(response) : Ok(response);
        }

        // POST: /api/MauSac
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MauSacDto dto)
        {
            var response = await _service.CreateAsync(dto);
            return response.Status == "error" ? BadRequest(response) : Ok(response);
        }

        // PUT: /api/MauSac/{maMauSac}
        [HttpPut("{maMauSac}")]
        public async Task<IActionResult> Update(int maMauSac, [FromBody] MauSacDto dto)
        {
            if (maMauSac != dto.MaMauSac)
            {
                return BadRequest(BaseResponse<MauSacDto>.Error(
                    new List<ErrorDetail> { new ErrorDetail("MaMauSac", "ID không khớp") },
                    "Cập nhật thất bại"));
            }

            var response = await _service.UpdateAsync(maMauSac, dto);
            return response.Status == "error" ? BadRequest(response) : Ok(response);
        }

        // DELETE: /api/MauSac/{maMauSac}
        [HttpDelete("{maMauSac}")]
        public async Task<IActionResult> Delete(int maMauSac)
        {
            var response = await _service.DeleteAsync(maMauSac);
            return response.Status == "error" ? BadRequest(response) : Ok(response);
        }
    }
}