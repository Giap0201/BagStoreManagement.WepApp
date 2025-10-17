using BagStore.Data;
using BagStore.Domain.Enums;
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

        public async Task<IEnumerable<DonHangPhanHoiDTO>> LayTatCaDonHangAsync()
        {
            var donHangs = await _donHangRepo.LayTatCaDonHangAsync();
            if (!donHangs.Any())
                return Enumerable.Empty<DonHangPhanHoiDTO>();
            return donHangs.Select(d => new DonHangPhanHoiDTO
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
        }

        public async Task<IEnumerable<DonHangPhanHoiDTO>> LayDonHangTheoKhachHangAsync(int maKhachHang)
        {
            var donHangs = await _donHangRepo.LayDonHangTheoKhachHangAsync(maKhachHang);
            if (!donHangs.Any())
                return Enumerable.Empty<DonHangPhanHoiDTO>();

            return donHangs.Select(d => new DonHangPhanHoiDTO
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
        }

        public async Task<DonHangPhanHoiDTO> TaoDonHangAsync(DonHangTaoDTO dto)
        {
            if (dto == null || dto.ChiTietDonHangs == null || dto.ChiTietDonHangs.Count == 0)
                throw new ArgumentException("Dữ liệu đơn hàng không hợp lệ.");

            var khachHang = await _context.KhachHangs.FindAsync(dto.MaKH)
                ?? throw new ArgumentException($"Khách hàng {dto.MaKH} không tồn tại.");

            var donHang = new Domain.Entities.DonHang
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
            await _donHangRepo.LuuAsync();

            foreach (var ct in dto.ChiTietDonHangs)
            {
                var chiTietSanPham = await _context.ChiTietSanPhams
                    .Include(c => c.SanPham)
                    .FirstOrDefaultAsync(p => p.MaChiTietSP == ct.MaChiTietSP)
                    ?? throw new ArgumentException($"Sản phẩm {ct.MaChiTietSP} không tồn tại.");

                if (chiTietSanPham.SoLuongTon < ct.SoLuong)
                    throw new InvalidOperationException($"Sản phẩm {chiTietSanPham.MaChiTietSP} tồn kho không đủ.");

                chiTietSanPham.SoLuongTon -= ct.SoLuong;
                _context.ChiTietSanPhams.Update(chiTietSanPham);

                var chiTiet = new Domain.Entities.ChiTietDonHang
                {
                    MaDonHang = donHang.MaDonHang,
                    MaChiTietSP = ct.MaChiTietSP,
                    SoLuong = ct.SoLuong,
                    GiaBan = ct.GiaBan
                };
                await _chiTietRepo.ThemAsync(chiTiet);
            }

            await _context.SaveChangesAsync();

            var chiTietDonHangPhanHoi = await _context.ChiTietDonHangs
                .Where(c => c.MaDonHang == donHang.MaDonHang)
                .Include(c => c.ChiTietSanPham).ThenInclude(sp => sp.SanPham)
                .Select(c => new DonHangChiTietPhanHoiDTO
                {
                    MaChiTietDH = c.MaChiTietDH,
                    TenSP = c.ChiTietSanPham.SanPham.TenSP,
                    SoLuong = c.SoLuong,
                    GiaBan = c.GiaBan
                })
                .ToListAsync();

            return new DonHangPhanHoiDTO
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
        }

        public async Task<DonHangPhanHoiDTO> CapNhatTrangThaiAsync(DonHangCapNhatTrangThaiDTO dto)
        {
            var mapTrangThai = new Dictionary<string, TrangThaiDonHang>(StringComparer.OrdinalIgnoreCase)
            {
                { "Chờ xử lý", TrangThaiDonHang.ChoXuLy },
                { "Đang giao hàng", TrangThaiDonHang.DangGiaoHang },
                { "Hoàn thành", TrangThaiDonHang.HoanThanh },
                { "Đã huỷ", TrangThaiDonHang.DaHuy }
            };

            if (!mapTrangThai.TryGetValue(dto.TrangThai, out var trangThaiEnum))
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

            return new DonHangPhanHoiDTO
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
        }
    }
}
