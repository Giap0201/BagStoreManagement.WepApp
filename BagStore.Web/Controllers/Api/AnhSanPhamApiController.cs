using BagStore.Web.Models.DTOs.SanPhams;
using BagStore.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BagStore.Web.Controllers.Api
{
    [Route("api/sanpham/{maSanPham}/[controller]")]
    [ApiController]
    [ValidateModel] // Tự động validate DataAnnotation trên DTO
    [Authorize(Roles = "Admin")]
    public class AnhSanPhamApiController : ControllerBase
    {
        private readonly IAnhSanPhamService _service;

        public AnhSanPhamApiController(IAnhSanPhamService service)
        {
            _service = service;
        }

        /// Lấy danh sách tất cả ảnh của một sản phẩm
        /// GET: /api/sanpham/{maSanPham}/AnhSanPhamApi
        [HttpGet]
        public async Task<IActionResult> GetAll(int maSanPham)
        {
            var response = await _service.GetBySanPhamIdAsync(maSanPham);
            return response.Status == "error" ? BadRequest(response) : Ok(response);
        }

        /// Lấy ảnh theo ID
        /// GET: /api/sanpham/{maSanPham}/AnhSanPhamApi/{maAnh}
        [HttpGet("{maAnh}")]
        public async Task<IActionResult> GetById(int maSanPham, int maAnh)
        {
            // Có thể validate xem ảnh có thuộc sản phẩm maSanPham không nếu muốn
            var response = await _service.GetByIdAsync(maAnh);
            return response.Status == "error" ? BadRequest(response) : Ok(response);
        }

        /// Thêm mới ảnh cho sản phẩm
        /// POST: /api/sanpham/{maSanPham}/AnhSanPhamApi
        [HttpPost]
        public async Task<IActionResult> Create(int maSanPham, [FromForm] AnhSanPhamRequestDto dto)
        {
            // Gán MaSP từ URL để đảm bảo client không gửi sai
            dto.MaSP = maSanPham;

            var response = await _service.CreateAsync(dto);
            return response.Status == "error" ? BadRequest(response) : Ok(response);
        }

        /// Cập nhật thông tin ảnh
        /// PUT: /api/sanpham/{maSanPham}/AnhSanPhamApi/{maAnh}
        [HttpPut("{maAnh}")]
        public async Task<IActionResult> Update(int maSanPham, int maAnh, [FromForm] AnhSanPhamRequestDto dto)
        {
            // Gán MaSP từ URL để tránh sai dữ liệu
            dto.MaSP = maSanPham;

            var response = await _service.UpdateAsync(maAnh, dto);
            return response.Status == "error" ? BadRequest(response) : Ok(response);
        }

        /// Xóa ảnh sản phẩm
        /// DELETE: /api/sanpham/{maSanPham}/AnhSanPhamApi/{maAnh}
        [HttpDelete("{maAnh}")]
        public async Task<IActionResult> Delete(int maSanPham, int maAnh)
        {
            // Có thể validate xem ảnh có thuộc sản phẩm maSanPham không nếu muốn
            var response = await _service.DeleteAsync(maAnh);
            return response.Status == "error" ? BadRequest(response) : Ok(response);
        }

        /// Đặt một ảnh làm ảnh chính
        /// POST: /api/sanpham/{maSanPham}/AnhSanPhamApi/{maAnh}/SetPrimary
        [HttpPost("{maAnh}/SetPrimary")]
        public async Task<IActionResult> SetPrimary(int maSanPham, int maAnh)
        {
            // (maSanPham từ URL không thực sự cần, vì service
            // đã có thể tự tìm maSP từ maAnh, nhưng giữ
            // nguyên cho cấu trúc route được nhất quán)

            var response = await _service.SetPrimaryAsync(maAnh);
            return response.Status == "error" ? BadRequest(response) : Ok(response);
        }
    }
}