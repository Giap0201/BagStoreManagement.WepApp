using BagStore.Data;
using BagStore.Web.Models.Entities.Enums;
using BagStore.Web.Models.DTOs.Request;
using BagStore.Web.Models.DTOs.Response;
using BagStore.Web.Repositories.Interfaces;
using BagStore.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BagStore.Web.Services.Implementations
{
    public class DonHangService : IDonHangService
    {
        private readonly BagStoreDbContext _context;
        private readonly IDonHangRepository _donHangRepo;
        private readonly IChiTietDonHangRepository _chiTietRepo;

        public DonHangService(
            BagStoreDbContext context,
            IDonHangRepository donHangRepo,
            IChiTietDonHangRepository chiTietRepo)
        {
            _context = context;
            _donHangRepo = donHangRepo;
            _chiTietRepo = chiTietRepo;
        }

        // ---------------- LẤY TẤT CẢ ĐƠN HÀNG ----------------
        public async Task<IEnumerable<DonHangResponse>> LayTatCaDonHangAsync()
        {
            var donHangs = await _donHangRepo.LayTatCaDonHangAsync();

            if (!donHangs.Any())
                return Enumerable.Empty<DonHangResponse>();

            return donHangs.Select(MapToDonHangResponse).ToList();
        }

        // ---------------- LẤY ĐƠN HÀNG THEO KHÁCH HÀNG ----------------
        public async Task<IEnumerable<DonHangResponse>> LayDonHangTheoKhachHangAsync(int maKhachHang)
        {
            var donHangs = await _donHangRepo.LayDonHangTheoKhachHangAsync(maKhachHang);

            if (!donHangs.Any())
                return Enumerable.Empty<DonHangResponse>();

            return donHangs.Select(MapToDonHangResponse).ToList();
        }

        // ---------------- TẠO ĐƠN HÀNG ----------------
        public async Task<DonHangResponse> TaoDonHangAsync(CreateDonHangRequest dto)
        {
            if (dto == null || dto.ChiTietDonHang == null || dto.ChiTietDonHang.Count == 0)
                throw new ArgumentException("Dữ liệu đơn hàng không hợp lệ.");

            var khachHang = await _context.KhachHangs.FindAsync(dto.MaKhachHang)
                ?? throw new ArgumentException($"Khách hàng {dto.MaKhachHang} không tồn tại.");

            var donHang = new Domain.Entities.DonHang
            {
                MaKH = dto.MaKhachHang,
                DiaChiGiaoHang = dto.DiaChiGiaoHang,
                PhuongThucThanhToan = dto.PhuongThucThanhToan,
                NgayDatHang = DateTime.Now,
                TongTien = dto.ChiTietDonHang.Sum(ct => ct.SoLuong * ct.GiaBan),
                TrangThai = "Chờ xử lý",
                TrangThaiThanhToan = "Chưa thanh toán",
                PhiGiaoHang = 0
            };

            await _donHangRepo.ThemAsync(donHang);
            await _donHangRepo.LuuAsync();

            foreach (var ct in dto.ChiTietDonHang)
            {
                var chiTietSanPham = await _context.ChiTietSanPhams
                    .Include(c => c.SanPham)
                    .FirstOrDefaultAsync(p => p.MaChiTietSP == ct.MaChiTietSanPham)
                    ?? throw new ArgumentException($"Sản phẩm {ct.MaChiTietSanPham} không tồn tại.");

                if (chiTietSanPham.SoLuongTon < ct.SoLuong)
                    throw new InvalidOperationException($"Sản phẩm {chiTietSanPham.MaChiTietSP} tồn kho không đủ.");

                chiTietSanPham.SoLuongTon -= ct.SoLuong;
                _context.ChiTietSanPhams.Update(chiTietSanPham);

                var chiTiet = new Domain.Entities.ChiTietDonHang
                {
                    MaDonHang = donHang.MaDonHang,
                    MaChiTietSP = ct.MaChiTietSanPham,
                    SoLuong = ct.SoLuong,
                    GiaBan = ct.GiaBan
                };

                await _chiTietRepo.ThemAsync(chiTiet);
            }

            await _context.SaveChangesAsync();

            var chiTietDonHangPhanHoi = await _context.ChiTietDonHangs
                .Where(c => c.MaDonHang == donHang.MaDonHang)
                .Include(c => c.ChiTietSanPham).ThenInclude(sp => sp.SanPham)
                .Select(c => new DonHangChiTietResponse
                {
                    MaChiTietDonHang = c.MaChiTietDH,
                    TenSanPham = c.ChiTietSanPham.SanPham.TenSP,
                    SoLuong = c.SoLuong,
                    GiaBan = c.GiaBan,
                    ThanhTien = c.SoLuong * c.GiaBan,
                    AnhSanPham = ""
                })
                .ToListAsync();

            return new DonHangResponse
            {
                MaDonHang = donHang.MaDonHang,
                TenKhachHang = khachHang.TenKH,
                NgayDatHang = donHang.NgayDatHang,
                TongTien = donHang.TongTien,
                TrangThai = donHang.TrangThai,
                PhuongThucThanhToan = donHang.PhuongThucThanhToan,
                TrangThaiThanhToan = donHang.TrangThaiThanhToan,
                DiaChiGiaoHang = donHang.DiaChiGiaoHang,
                ChiTietDonHang = chiTietDonHangPhanHoi
            };
        }

        // ---------------- CẬP NHẬT TRẠNG THÁI ----------------
        public async Task<DonHangResponse> CapNhatTrangThaiAsync(UpdateDonHangStatusRequest dto)
        {
            var mapTrangThai = new Dictionary<string, TrangThaiDonHang>(StringComparer.OrdinalIgnoreCase)
            {
                { "Chờ xử lý", TrangThaiDonHang.ChoXuLy },
                { "Đang giao hàng", TrangThaiDonHang.DangGiaoHang },
                { "Hoàn thành", TrangThaiDonHang.HoanThanh },
                { "Đã huỷ", TrangThaiDonHang.DaHuy }
            };

            if (!mapTrangThai.ContainsKey(dto.TrangThai))
                throw new ArgumentException("Trạng thái đơn hàng không hợp lệ.");

            var donHang = await _context.DonHangs
                .Include(d => d.KhachHang)
                .FirstOrDefaultAsync(d => d.MaDonHang == dto.MaDonHang)
                ?? throw new KeyNotFoundException("Đơn hàng không tồn tại.");

            donHang.TrangThai = dto.TrangThai;
            _context.DonHangs.Update(donHang);
            await _context.SaveChangesAsync();

            var chiTietDonHangs = await _context.ChiTietDonHangs
                .Where(ct => ct.MaDonHang == donHang.MaDonHang)
                .Include(ct => ct.ChiTietSanPham).ThenInclude(sp => sp.SanPham)
                .Select(ct => new DonHangChiTietResponse
                {
                    MaChiTietDonHang = ct.MaChiTietDH,
                    TenSanPham = ct.ChiTietSanPham.SanPham.TenSP,
                    SoLuong = ct.SoLuong,
                    GiaBan = ct.GiaBan,
                    ThanhTien = ct.SoLuong * ct.GiaBan,
                    AnhSanPham = ""
                })
                .ToListAsync();

            return new DonHangResponse
            {
                MaDonHang = donHang.MaDonHang,
                TenKhachHang = donHang.KhachHang.TenKH,
                NgayDatHang = donHang.NgayDatHang,
                TongTien = donHang.TongTien,
                TrangThai = donHang.TrangThai,
                PhuongThucThanhToan = donHang.PhuongThucThanhToan,
                TrangThaiThanhToan = donHang.TrangThaiThanhToan,
                DiaChiGiaoHang = donHang.DiaChiGiaoHang,
                ChiTietDonHang = chiTietDonHangs
            };
        }

        // ---------------- HÀM MAP ENTITY → DTO ----------------
        private static DonHangResponse MapToDonHangResponse(Domain.Entities.DonHang d)
        {
            return new DonHangResponse
            {
                MaDonHang = d.MaDonHang,
                TenKhachHang = d.KhachHang?.TenKH ?? "Khách",
                NgayDatHang = d.NgayDatHang,
                TongTien = d.TongTien,
                TrangThai = d.TrangThai,
                PhuongThucThanhToan = d.PhuongThucThanhToan,
                TrangThaiThanhToan = d.TrangThaiThanhToan,
                DiaChiGiaoHang = d.DiaChiGiaoHang,
                ChiTietDonHang = d.ChiTietDonHangs?.Select(ct => new DonHangChiTietResponse
                {
                    MaChiTietDonHang = ct.MaChiTietDH,
                    TenSanPham = ct.ChiTietSanPham?.SanPham?.TenSP ?? "Sản phẩm",
                    SoLuong = ct.SoLuong,
                    GiaBan = ct.GiaBan,
                    ThanhTien = ct.SoLuong * ct.GiaBan,
                    AnhSanPham = ""
                }).ToList() ?? new List<DonHangChiTietResponse>()
            };
        }
    }
}
