using BagStore.Data;
using BagStore.Domain.Enums;
using BagStore.Web.Models.DTOs.Request;
using BagStore.Web.Models.DTOs.Response;
using BagStore.Web.Repositories.Interfaces;
using BagStore.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BagStore.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonHangApiController : ControllerBase
    {
        private readonly IDonHangService _donHangService;

        public DonHangApiController(IDonHangService donHangService)
        {
            _donHangService = donHangService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DonHangPhanHoiDTO>>> LayTatCaDonHang()
        {
            try
            {
                var result = await _donHangService.LayTatCaDonHangAsync(); // thêm method service/repo nếu chưa có
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi server");
            }
        }


        [HttpGet("KhachHang/{maKhachHang}")]
        public async Task<ActionResult<IEnumerable<DonHangPhanHoiDTO>>> LayDonHangTheoKhachHang(int maKhachHang)
        {
            var result = await _donHangService.LayDonHangTheoKhachHangAsync(maKhachHang);
            if (!result.Any())
                return NotFound("Chưa có đơn hàng.");
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<DonHangPhanHoiDTO>> TaoDonHang([FromBody] DonHangTaoDTO dto)
        {
            try
            {
                var result = await _donHangService.TaoDonHangAsync(dto);
                return CreatedAtAction(nameof(LayDonHangTheoKhachHang),
                    new { maKhachHang = dto.MaKH }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("CapNhatTrangThai")]
        public async Task<ActionResult<DonHangPhanHoiDTO>> CapNhatTrangThai([FromBody] DonHangCapNhatTrangThaiDTO dto)
        {
            try
            {
                var result = await _donHangService.CapNhatTrangThaiAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
