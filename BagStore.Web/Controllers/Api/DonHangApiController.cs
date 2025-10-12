using BagStore.Data;
using BagStore.Web.Models.DTOs;
using BagStore.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BagStore.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonHangApiController : ControllerBase
    {
        private readonly BagStoreDbContext _context;
        private readonly IDonHangRepository _donHangRepo;
        private readonly IChiTietDonHangRepository _chiTietRepo;

        public DonHangApiController(
            BagStoreDbContext context,
            IDonHangRepository donHangRepo,
            IChiTietDonHangRepository chiTietRepo)
        {
            _context = context;                 // đây là _context
            _donHangRepo = donHangRepo;
            _chiTietRepo = chiTietRepo;
        }

        // GET: api/DonHangApi/KhachHang/5
        [HttpGet("KhachHang/{maKhachHang}")]
        public async Task<ActionResult<IEnumerable<DonHangPhanHoiDTO>>> LayDonHangTheoKhachHang(int maKhachHang)
        {
            var donHangs = await _donHangRepo.LayDonHangTheoKhachHangAsync(maKhachHang);
            if (!donHangs.Any())
                return NotFound("Chưa có đơn hàng.");

            // Mapping Entity → DTO
            var result = donHangs.Select(d => new DonHangPhanHoiDTO
            {
                MaDonHang = d.MaDonHang,
                TenKH = d.KhachHang?.TenKH ?? "Khách",
                NgayDatHang = d.NgayDatHang,
                TongTien = d.TongTien,
                TrangThai = d.TrangThai,
                PhuongThucThanhToan = d.PhuongThucThanhToan,
                TrangThaiThanhToan = d.TrangThaiThanhToan,
                ChiTietDonHangs = d.ChiTietDonHangs.Select(ct => new DonHangChiTietPhanHoiDTO
                {
                    MaChiTietDH = ct.MaChiTietDH,
                    TenSP = ct.ChiTietSanPham?.SanPham?.TenSP ?? "Sản phẩm",
                    SoLuong = ct.SoLuong,
                    GiaBan = ct.GiaBan
                }).ToList()
            }).ToList();

            return Ok(result);
        }

        // POST: api/DonHangApi
        [HttpPost]
        public async Task<ActionResult> TaoDonHang([FromBody] DonHangTaoDTO dto)
        {
            if (dto == null || dto.ChiTietDonHangs.Count == 0)
                return BadRequest("Dữ liệu đơn hàng không hợp lệ.");

            var donHang = new BagStore.Domain.Entities.DonHang
            {
                MaKH = dto.MaKH,
                DiaChiGiaoHang = dto.DiaChiGiaoHang,
                PhuongThucThanhToan = dto.PhuongThucThanhToan,
                NgayDatHang = DateTime.Now,
                TongTien = dto.ChiTietDonHangs.Sum(ct => ct.SoLuong * ct.GiaBan),
                TrangThai = "Chờ xác nhận",
                TrangThaiThanhToan = "Chưa thanh toán",
                PhiGiaoHang = 0
            };

            await _donHangRepo.ThemAsync(donHang);
            await _donHangRepo.LuuAsync();

            foreach (var ct in dto.ChiTietDonHangs)
            {
                // Lấy ChiTietSanPham hiện tại
                var chiTietSanPham = await _context.ChiTietSanPhams
                    .FirstOrDefaultAsync(p => p.MaChiTietSP == ct.MaChiTietSP);

                if (chiTietSanPham == null)
                    return BadRequest($"Sản phẩm {ct.MaChiTietSP} không tồn tại.");

                if (chiTietSanPham.SoLuongTon < ct.SoLuong)
                    return BadRequest($"Sản phẩm {chiTietSanPham.MaChiTietSP} tồn kho không đủ.");

                // Giảm tồn kho
                chiTietSanPham.SoLuongTon -= ct.SoLuong;
                _context.ChiTietSanPhams.Update(chiTietSanPham);

                // Thêm chi tiết đơn hàng
                var chiTiet = new BagStore.Domain.Entities.ChiTietDonHang
                {
                    MaDonHang = donHang.MaDonHang,
                    MaChiTietSP = ct.MaChiTietSP,
                    SoLuong = ct.SoLuong,
                    GiaBan = ct.GiaBan
                };
                await _chiTietRepo.ThemAsync(chiTiet);
            }

            await _chiTietRepo.LuuAsync();
            await _context.SaveChangesAsync(); // lưu thay đổi tồn kho

            return CreatedAtAction(nameof(LayDonHangTheoKhachHang), new { maKhachHang = dto.MaKH }, donHang.MaDonHang);
        }


        // PUT: api/DonHangApi/CapNhatTrangThai
        [HttpPut("CapNhatTrangThai")]
        public async Task<ActionResult> CapNhatTrangThai([FromBody] DonHangCapNhatTrangThaiDTO dto)
        {
            var donHang = await _donHangRepo.LayTheoIdAsync(dto.MaDonHang);
            if (donHang == null) return NotFound("Đơn hàng không tồn tại.");

            donHang.TrangThai = dto.TrangThai;
            await _donHangRepo.CapNhatAsync(donHang);
            await _donHangRepo.LuuAsync();

            return Ok("Cập nhật trạng thái thành công.");
        }
    }
}
