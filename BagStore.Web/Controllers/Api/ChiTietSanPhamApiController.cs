using BagStore.Models.Common;
using BagStore.Web.Models.DTOs.SanPhams;
using BagStore.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BagStore.Web.Controllers.Api
{
    [Route("api/sanpham/{maSanPham}/[controller]")]
    [ApiController]
    [ValidateModel] // Tự động validate DataAnnotation trên DTO
    [Authorize]
    public class ChiTietSanPhamApiController : ControllerBase
    {
        private readonly IChiTietSanPhamService _service;

        public ChiTietSanPhamApiController(IChiTietSanPhamService service)
        {
            _service = service;
        }

        /// Lấy danh sách tất cả biến thể của một sản phẩm
        /// GET: /api/sanpham/{maSanPham}/ChiTietSanPhamApi

        [HttpGet]
        public async Task<IActionResult> GetAll(int maSanPham)
        {
            var response = await _service.GetBySanPhamIdAsync(maSanPham);
            return response.Status == "error" ? BadRequest(response) : Ok(response);
        }

        /// Lấy chi tiết biến thể theo ID
        /// GET: /api/sanpham/{maSanPham}/ChiTietSanPhamApi/{maChiTietSP}

        [HttpGet("{maChiTietSP}")]
        public async Task<IActionResult> GetById(int maSanPham, int maChiTietSP)
        {
            // Có thể validate xem biến thể có thuộc sản phẩm maSanPham không nếu muốn
            var response = await _service.GetByIdAsync(maChiTietSP);
            return response.Status == "error" ? BadRequest(response) : Ok(response);
        }

        /// Tạo mới biến thể cho sản phẩm
        /// POST: /api/sanpham/{maSanPham}/ChiTietSanPhamApi
        /// 

        [HttpGet("/api/ChiTietSanPhamApi/{maChiTietSP}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByIdGlobal(int maChiTietSP)
        {
            var response = await _service.GetByIdAsync(maChiTietSP);
            return response.Status == "error" ? BadRequest(response) : Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int maSanPham, [FromBody] ChiTietSanPhamRequestDto dto)
        {
            // Gán MaSanPhan từ URL để tránh client gửi sai
            dto.MaSanPhan = maSanPham;

            var response = await _service.CreateAsync(dto);
            return response.Status == "error" ? BadRequest(response) : Ok(response);
        }

        /// Cập nhật biến thể
        /// PUT: /api/sanpham/{maSanPham}/ChiTietSanPhamApi/{maChiTietSP}

        [HttpPut("{maChiTietSP}")]
        public async Task<IActionResult> Update(int maSanPham, int maChiTietSP, [FromBody] ChiTietSanPhamRequestDto dto)
        {
            // Gán MaSanPhan từ URL để đảm bảo tính nhất quán
            dto.MaSanPhan = maSanPham;

            var response = await _service.UpdateAsync(maChiTietSP, dto);
            return response.Status == "error" ? BadRequest(response) : Ok(response);
        }

        /// Xóa biến thể sản phẩm
        /// DELETE: /api/sanpham/{maSanPham}/ChiTietSanPhamApi/{maChiTietSP}

        [HttpDelete("{maChiTietSP}")]
        public async Task<IActionResult> Delete(int maSanPham, int maChiTietSP)
        {
            // Có thể validate xem biến thể có thuộc sản phẩm maSanPham không nếu muốn
            var response = await _service.DeleteAsync(maChiTietSP);
            return response.Status == "error" ? BadRequest(response) : Ok(response);
        }
    }
}