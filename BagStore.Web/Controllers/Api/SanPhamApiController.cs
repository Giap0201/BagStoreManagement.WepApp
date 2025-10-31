using BagStore.Models.Common;
using BagStore.Web.Models.DTOs.SanPhams;
using BagStore.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BagStore.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [ValidateModel] // Tự động validate DataAnnotation trên DTO, trả BaseResponse nếu lỗi
    [Authorize(Roles = "Admin")]
    public class SanPhamApiController : ControllerBase
    {
        private readonly ISanPhamService _service;

        public SanPhamApiController(ISanPhamService service)
        {
            _service = service;
        }

        /// Lấy danh sách tất cả sản phẩm
        /// GET: /api/SanPhamApi

        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    var response = await _service.GetAllAsync();
        //    // Nếu thất bại thì BadRequest, thành công trả Ok
        //    return response.Status == "error" ? BadRequest(response) : Ok(response);
        //}

        /// Lấy sản phẩm theo ID
        /// GET: /api/SanPhamApi/{maSanPham}

        [HttpGet("{maSanPham}")]
        public async Task<IActionResult> GetById(int maSanPham)
        {
            var response = await _service.GetByIdAsync(maSanPham);
            return response.Status == "error" ? BadRequest(response) : Ok(response);
        }

        /// Tạo mới sản phẩm
        /// POST: /api/SanPhamApi
        /// Sử dụng [FromForm] để bind IFormFile

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] SanPhamRequestDto dto)
        {
            var response = await _service.CreateAsync(dto);
            return response.Status == "error" ? BadRequest(response) : Ok(response);
        }

        /// Cập nhật sản phẩm
        /// PUT: /api/SanPhamApi/{maSanPham}
        /// Sử dụng [FromForm] để bind IFormFile (ảnh sản phẩm)
        /// ID sản phẩm lấy từ URL, không lấy từ DTO

        [HttpPut("{maSanPham}")]
        public async Task<IActionResult> Update(int maSanPham, [FromForm] SanPhamRequestDto dto)
        {
            var response = await _service.UpdateAsync(maSanPham, dto);
            return response.Status == "error" ? BadRequest(response) : Ok(response);
        }

        /// Xóa sản phẩm theo ID
        /// DELETE: /api/SanPhamApi/{maSanPham}

        [HttpDelete("{maSanPham}")]
        public async Task<IActionResult> Delete(int maSanPham)
        {
            var response = await _service.DeleteAsync(maSanPham);
            return response.Status == "error" ? BadRequest(response) : Ok(response);
        }

        //lay ra danh sach cac san pham,co the phan trang va tim kiem
        //page: So trang (mac dinh 1)
        //pageSize: so luong muc (mac dinh 10)
        // GET: /api/SanPhamApi?page=1&pageSize=5 (Lấy trang 1, 5 mục)
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? page, [FromQuery] int? pageSize, [FromQuery] string? search)
        {
            // Nếu không cung cấp page hoặc pageSize, gọi phương thức GetAllAsync() cũ
            if (!page.HasValue || !pageSize.HasValue)
            {
                var response = await _service.GetAllAsync();
                return response.Status == "error" ? BadRequest(response) : Ok(response);
            }

            // Nếu có phân trang, gọi phương thức mới
            var pagedResponse = await _service.GetAllPagingAsync(page.Value, pageSize.Value, search);
            return pagedResponse.Status == "error" ? BadRequest(pagedResponse) : Ok(pagedResponse);
        }
    }
}