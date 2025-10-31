using BagStore.Web.Models.DTOs.Requests;
using BagStore.Web.Models.DTOs.Response;
using BagStore.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BagStore.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        [Authorize(Roles = "Admin")]
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
        [HttpGet("khachhang/{userId}")]
        public async Task<ActionResult<IEnumerable<DonHangResponse>>> LayDonHangTheoKhachHang(string userId)
        {
            try
            {
                var orders = await _donHangService.LayDonHangTheoUserAsync(userId);
                if (orders == null || !orders.Any())
                    return NotFound("Bạn chưa có đơn hàng nào!");

                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy đơn hàng cho user {userId}", userId);
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
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                var response = await _donHangService.TaoDonHangAsync(dto, userId);
                return Ok(response);
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
            try
            {
                if (dto == null)
                    return BadRequest(new { success = false, message = "Dữ liệu yêu cầu không hợp lệ." });

                var result = await _donHangService.CapNhatTrangThaiAsync(dto);

                return Ok(result);
            }
            // ❌ Lỗi dữ liệu đầu vào
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            // ❌ Đơn hàng không tồn tại
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            // ❌ Cấm thao tác (ví dụ: đơn đã hoàn thành hoặc đã hủy)
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            // ❌ Lỗi không mong đợi
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi không xác định khi cập nhật trạng thái đơn hàng.");
                return StatusCode(500, "Lỗi không xác định");
            }
        }

        [HttpGet("{maDH:int}")]
        public async Task<IActionResult> GetById(int maDH)
        {
            var donHang = await _donHangService.GetByIdAsync(maDH);
            if (donHang == null)
                return NotFound(new { message = "Không tìm thấy đơn hàng." });

            return Ok(donHang);
        }
    }
}
