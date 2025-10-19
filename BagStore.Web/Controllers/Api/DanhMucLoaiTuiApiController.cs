using BagStore.Models.Common;
using BagStore.Services.Interfaces;
using BagStore.Web.Models.Common;
using BagStore.Web.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ProductManagementApp.Controllers.Api
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

        //GET: Lấy tất cả
        //GET: /api/DanhMucLoaiTui
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _service.GetAllAsync();
            return Ok(response); //BaseResponse<List<DanhMucLoaiTuiDto>>
        }

        // Lấy loại túi theo ID
        //GET: /api/DanhMucLoaiTui/{id}
        [HttpGet("{maLoaiTui}")]
        public async Task<IActionResult> GetById(int maLoaiTui)
        {
            var response = await _service.GetByIdAsync(maLoaiTui);
            return Ok(response);
        }

        //Thêm mới loại túi
        //POST: /api/DanhMucLoaiTui
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DanhMucLoaiTuiDto dto)
        {
            // Gọi service thêm mới và trả về BaseResponse<DanhMucLoaiTuiDto>
            var response = await _service.CreateAsync(dto);
            return Ok(response);
        }

        //Cập nhật loại túi theo ID
        //PUT: /api/DanhMucLoaiTui/{id}
        [HttpPut("{maLoaiTui}")]
        public async Task<IActionResult> Update(int maLoaiTui, [FromBody] DanhMucLoaiTuiDto dto)
        {
            // Kiểm tra ID trong URL có khớp với ID trong body không
            if (maLoaiTui != dto.MaLoaiTui)
            {
                return BadRequest(BaseResponse<DanhMucLoaiTuiDto>.Error(
                    new List<ErrorDetail> { new ErrorDetail("MaLoaiTui", "ID không khớp") },
                    "Cập nhật thất bại"));
            }

            var response = await _service.UpdateAsync(maLoaiTui, dto);
            return Ok(response); // Trả về BaseResponse<DanhMucLoaiTuiDto>
        }

        //Xóa loại túi theo ID
        //DELETE: /api/DanhMucLoaiTui/{id}
        [HttpDelete("{maLoaiTui}")]
        public async Task<IActionResult> Delete(int maLoaiTui)
        {
            var response = await _service.DeleteAsync(maLoaiTui);
            return Ok(response); // Trả về BaseResponse<bool>
        }
    }
}