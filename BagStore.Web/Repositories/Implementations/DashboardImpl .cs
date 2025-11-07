using BagStore.Data;
using BagStore.Web.Areas.Admin.Models;
using BagStore.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BagStore.Web.Repositories.Implementations
{
    public class DashboardImpl : IDashboardRepository
    {
        private readonly BagStoreDbContext _context;

        public DashboardImpl(BagStoreDbContext context)
        {
            _context = context;
        }

        //bieu do doanh thu theo thang
        public async Task<BieuDoDoanhThuDTO> LayBieuDoDoanhThuAsync(int nam)
        {
            var labels = Enumerable.Range(1, 12).Select(x => $"Tháng {x}").ToList();
            var values = new List<decimal>();
            for (int i = 1; i <= 12; i++)
            {
                var doanhThu = await _context.DonHangs
                    .Where(x => x.TrangThai == "Hoàn Thành"
                                && x.NgayDatHang.Month == i
                                && x.NgayDatHang.Year == nam)
                    .SumAsync(x => (decimal?)x.TongTien ?? 0);
                values.Add(doanhThu);
            }
            return new BieuDoDoanhThuDTO
            {
                NhanThang = labels,
                GiaTriDoanhThu = values
            };
        }

        //bieu do trang thai don hang
        public async Task<BieuDoTrangThaiDonHangDTO> LayBieuDoTrangThaiDonHangAsync()
        {
            var soChoXuLy = await _context.DonHangs.CountAsync(x => x.TrangThai == "Chờ Xử Lý");
            var soDangGiao = await _context.DonHangs.CountAsync(x => x.TrangThai == "Đang Giao Hàng");
            var soHoanThanh = await _context.DonHangs.CountAsync(x => x.TrangThai == "Hoàn Thành");
            var soDaHuy = await _context.DonHangs.CountAsync(x => x.TrangThai == "Đã Huỷ");
            return new BieuDoTrangThaiDonHangDTO
            {
                SoChoXuLy = soChoXuLy,
                SoDangGiao = soDangGiao,
                SoHoanThanh = soHoanThanh,
                SoDaHuy = soDaHuy
            };
        }

        //danh sach don hang gan day
        public async Task<List<DonHangGanDayDTO>> LayDanhSachDonHangGanDayAsync(int soLuong = 5)
        {
            var result = await _context.DonHangs
                .Include(d => d.KhachHang)
                .OrderByDescending(d => d.NgayDatHang)
                .Take(soLuong)
                .Select(d => new DonHangGanDayDTO
                {
                    MaDonHang = d.MaDonHang,
                    TenKhachHang = d.KhachHang.TenKH,
                    NgayDatHang = d.NgayDatHang,
                    TongTien = d.TongTien,
                    TrangThai = d.TrangThai
                }).ToListAsync();
            return result;
        }

        //thong ke san pham ban chay
        public async Task<List<SanPhamBanChayDTO>> LayDanhSachSanPhamBanChayAsync(int soLuong = 5)
        {
            var result = await _context.ChiTietDonHangs
                .Where(c => c.DonHang.TrangThai == "Hoàn Thành")
                .GroupBy(c => new
                {
                    c.ChiTietSanPham.SanPham.MaSP,
                    c.ChiTietSanPham.SanPham.TenSP
                })
                .Select(g => new SanPhamBanChayDTO
                {
                    MaSanPham = g.Key.MaSP,
                    TenSanPham = g.Key.TenSP,
                    SoLuongBan = g.Sum(x => x.SoLuong),
                    DoanhThu = g.Sum(x => x.SoLuong * x.GiaBan)
                })
                .OrderByDescending(x => x.SoLuongBan)
                .Take(5)
                .ToListAsync();
            return result;
        }

        //thong ke tong quan
        public async Task<ThongKeTongQuanDTO> LayThongKeTongQuanAsync()
        {
            var thangHienTai = DateTime.Now.Month;
            var tongSanPham = await _context.SanPhams.CountAsync();
            var tongDonHang = await _context.DonHangs.CountAsync();
            var tongKhachHang = await _context.KhachHangs.CountAsync();

            var tongDoanhThuThang = await _context.DonHangs
                .Where(dh => dh.TrangThai == "Hoàn Thành" && dh.NgayDatHang.Month == thangHienTai)
                .SumAsync(x => (decimal?)x.TongTien ?? 0);
            return new ThongKeTongQuanDTO
            {
                TongSanPham = tongSanPham,
                TongDonHang = tongDonHang,
                TongKhachHang = tongKhachHang,
                TongDoanhThuThang = tongDoanhThuThang
            };
        }
    }
}