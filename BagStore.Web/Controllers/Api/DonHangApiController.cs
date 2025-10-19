using BagStore.Web.Models.DTOs.Request;
using BagStore.Web.Models.DTOs.Response;
using BagStore.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BagStore.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonHangApiController : ControllerBase
    {
        private readonly IDonHangService _donHangService;
        private readonly ILogger<DonHangApiController> _logger;

        public DonHangApiController(IDonHangService donHangService, ILogger<DonHangApiController> logger)
        {
            _donHangService = donHangService;
            _logger = logger;
        }

        /// <summary>
        /// Lấy tất cả đơn hàng (chỉ Admin)
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DonHangResponse>>> LayTatCaDonHang()
        {
            try
            {
                var orders = await _donHangService.LayTatCaDonHangAsync();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách đơn hàng");
                return StatusCode(500, "Đã xảy ra lỗi phía máy chủ.");
            }
        }

        /// <summary>
        /// Lấy danh sách đơn hàng của một khách hàng
        /// </summary>
        [HttpGet("khachhang/{maKhachHang:int}")]
        public async Task<ActionResult<IEnumerable<DonHangResponse>>> LayDonHangTheoKhachHang(int maKhachHang)
        {
            try
            {
                var orders = await _donHangService.LayDonHangTheoKhachHangAsync(maKhachHang);
                if (orders == null || !orders.Any())
                    return NotFound("Không tìm thấy đơn hàng nào cho khách hàng này.");

                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy đơn hàng cho khách hàng {maKhachHang}", maKhachHang);
                return StatusCode(500, "Đã xảy ra lỗi phía máy chủ.");
            }
        }

        /// <summary>
        /// Tạo mới một đơn hàng
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<DonHangResponse>> TaoDonHang([FromBody] CreateDonHangRequest dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var order = await _donHangService.TaoDonHangAsync(dto);
                return CreatedAtAction(nameof(LayDonHangTheoKhachHang),
                    new { maKhachHang = dto.MaKhachHang }, order);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Dữ liệu không hợp lệ khi tạo đơn hàng");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo đơn hàng");
                return StatusCode(500, "Đã xảy ra lỗi phía máy chủ.");
            }
        }

        /// <summary>
        /// Cập nhật trạng thái đơn hàng (Admin)
        /// </summary>
        [HttpPut("capnhat-trangthai")]
        public async Task<ActionResult<DonHangResponse>> CapNhatTrangThai([FromBody] UpdateDonHangStatusRequest dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var order = await _donHangService.CapNhatTrangThaiAsync(dto);
                if (order == null)
                    return NotFound("Không tìm thấy đơn hàng cần cập nhật.");

                return Ok(order);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Trạng thái cập nhật không hợp lệ");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật trạng thái đơn hàng");
                return StatusCode(500, "Đã xảy ra lỗi phía máy chủ.");
            }
        }
    }
}
