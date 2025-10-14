using BagStore.Data;
using BagStore.Domain.Enums;
using BagStore.Web.Models.DTOs.Request;
using BagStore.Web.Models.DTOs.Response;
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

        [HttpPost]
        public async Task<ActionResult<DonHangPhanHoiDTO>> TaoDonHang([FromBody] DonHangTaoDTO dto)
        {
            // 1️⃣ Kiểm tra dữ liệu đầu vào
            if (dto == null || dto.ChiTietDonHangs == null || dto.ChiTietDonHangs.Count == 0)
                return BadRequest("Dữ liệu đơn hàng không hợp lệ.");

            var khachHang = await _context.KhachHangs.FindAsync(dto.MaKH);
            if (khachHang == null)
                return BadRequest($"Khách hàng {dto.MaKH} không tồn tại.");

            // 2️⃣ Tạo đơn hàng mới
            var donHang = new BagStore.Domain.Entities.DonHang
            {
                MaKH = dto.MaKH,
                DiaChiGiaoHang = dto.DiaChiGiaoHang,
                PhuongThucThanhToan = dto.PhuongThucThanhToan,
                NgayDatHang = DateTime.Now,
                TongTien = dto.ChiTietDonHangs.Sum(ct => ct.SoLuong * ct.GiaBan),
                TrangThai = "Chờ xử lý",
                TrangThaiThanhToan = "Chưa thanh toán",
                PhiGiaoHang = 0
            };

            await _donHangRepo.ThemAsync(donHang);
            await _donHangRepo.LuuAsync(); // Lưu để có MaDonHang

            // 3️⃣ Thêm chi tiết đơn hàng
            foreach (var ct in dto.ChiTietDonHangs)
            {
                var chiTietSanPham = await _context.ChiTietSanPhams
                    .Include(c => c.SanPham)
                    .FirstOrDefaultAsync(p => p.MaChiTietSP == ct.MaChiTietSP);

                if (chiTietSanPham == null)
                    return BadRequest($"Sản phẩm {ct.MaChiTietSP} không tồn tại.");

                if (chiTietSanPham.SoLuongTon < ct.SoLuong)
                    return BadRequest($"Sản phẩm {chiTietSanPham.MaChiTietSP} tồn kho không đủ.");

                // Giảm tồn kho
                chiTietSanPham.SoLuongTon -= ct.SoLuong;
                _context.ChiTietSanPhams.Update(chiTietSanPham);

                // Thêm chi tiết đơn hàng (chưa cần lấy MaChiTietDH ngay)
                var chiTiet = new BagStore.Domain.Entities.ChiTietDonHang
                {
                    MaDonHang = donHang.MaDonHang,
                    MaChiTietSP = ct.MaChiTietSP,
                    SoLuong = ct.SoLuong,
                    GiaBan = ct.GiaBan
                };

                await _chiTietRepo.ThemAsync(chiTiet);
            }

            // 4️⃣ Lưu tất cả thay đổi một lần — sinh mã tự tăng cho ChiTietDonHang
            await _context.SaveChangesAsync();

            // 5️⃣ Truy vấn lại danh sách chi tiết để phản hồi
            var chiTietDonHangPhanHoi = await _context.ChiTietDonHangs
                .Where(c => c.MaDonHang == donHang.MaDonHang)
                .Include(c => c.ChiTietSanPham)
                    .ThenInclude(sp => sp.SanPham)
                .Select(c => new DonHangChiTietPhanHoiDTO
                {
                    MaChiTietDH = c.MaChiTietDH,
                    TenSP = c.ChiTietSanPham.SanPham.TenSP,
                    SoLuong = c.SoLuong,
                    GiaBan = c.GiaBan
                })
                .ToListAsync();

            // 6️⃣ Tạo DTO phản hồi
            var response = new DonHangPhanHoiDTO
            {
                MaDonHang = donHang.MaDonHang,
                TenKH = khachHang.TenKH,
                NgayDatHang = donHang.NgayDatHang,
                TongTien = donHang.TongTien,
                TrangThai = donHang.TrangThai,
                PhuongThucThanhToan = donHang.PhuongThucThanhToan,
                TrangThaiThanhToan = donHang.TrangThaiThanhToan,
                ChiTietDonHangs = chiTietDonHangPhanHoi
            };

            // 7️⃣ Trả về kết quả
            return CreatedAtAction(nameof(LayDonHangTheoKhachHang),
                new { maKhachHang = donHang.MaKH },
                response);
        }




        // PUT: api/DonHangApi/CapNhatTrangThai
        [HttpPut("CapNhatTrangThai")]
        public async Task<ActionResult<DonHangPhanHoiDTO>> CapNhatTrangThai([FromBody] DonHangCapNhatTrangThaiDTO dto)
        {
            if (dto == null)
                return BadRequest("Dữ liệu không hợp lệ.");

            // 1️⃣ Map tiếng Việt -> Enum
            var mapTrangThai = new Dictionary<string, TrangThaiDonHang>(StringComparer.OrdinalIgnoreCase)
            {
                { "Chờ xử lý", TrangThaiDonHang.ChoXuLy },
                { "Đang giao hàng", TrangThaiDonHang.DangGiaoHang },
                { "Hoàn thành", TrangThaiDonHang.HoanThanh },
                { "Đã huỷ", TrangThaiDonHang.DaHuy }
            };

            if (!mapTrangThai.TryGetValue(dto.TrangThai, out var trangThaiEnum))
                return BadRequest("Trạng thái đơn hàng không hợp lệ.");

            // 2️⃣ Lấy đơn hàng
            var donHang = await _context.DonHangs
                .Include(d => d.KhachHang)
                .FirstOrDefaultAsync(d => d.MaDonHang == dto.MaDonHang);

            if (donHang == null)
                return NotFound("Đơn hàng không tồn tại.");

            // 3️⃣ Cập nhật trạng thái (ghi tiếng Việt để khớp DB)
            donHang.TrangThai = dto.TrangThai;
            _context.DonHangs.Update(donHang);
            await _context.SaveChangesAsync();

            // 4️⃣ Chuẩn bị phản hồi
            var chiTietDonHangs = await _context.ChiTietDonHangs
                .Where(ct => ct.MaDonHang == donHang.MaDonHang)
                .Include(ct => ct.ChiTietSanPham)
                    .ThenInclude(sp => sp.SanPham)
                .Select(ct => new DonHangChiTietPhanHoiDTO
                {
                    MaChiTietDH = ct.MaChiTietDH,
                    TenSP = ct.ChiTietSanPham.SanPham.TenSP,
                    SoLuong = ct.SoLuong,
                    GiaBan = ct.GiaBan
                })
                .ToListAsync();

            var response = new DonHangPhanHoiDTO
            {
                MaDonHang = donHang.MaDonHang,
                TenKH = donHang.KhachHang.TenKH,
                NgayDatHang = donHang.NgayDatHang,
                TongTien = donHang.TongTien,
                TrangThai = donHang.TrangThai,
                PhuongThucThanhToan = donHang.PhuongThucThanhToan,
                TrangThaiThanhToan = donHang.TrangThaiThanhToan,
                ChiTietDonHangs = chiTietDonHangs
            };

            return Ok(response);
        }


    }
}
