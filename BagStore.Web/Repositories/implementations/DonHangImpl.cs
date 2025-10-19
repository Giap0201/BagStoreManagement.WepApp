using BagStore.Data;
using BagStore.Domain.Entities;
using BagStore.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BagStore.Web.Repositories.implementations
{
    public class DonHangImpl : GenericImpl<DonHang>, IDonHangRepository
    {
        public DonHangImpl(BagStoreDbContext context) : base(context) { }

        // Lấy đơn hàng theo mã khách hàng
        public async Task<IEnumerable<DonHang>> LayDonHangTheoKhachHangAsync(int maKH)
        {
            return await _dbSet
                .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(ct => ct.ChiTietSanPham)
                        .ThenInclude(sp => sp.SanPham)
                .Include(d => d.KhachHang)
                .Where(d => d.MaKH == maKH)
                .ToListAsync();
        }

        // Lấy đơn hàng theo trạng thái
        public async Task<IEnumerable<DonHang>> LayDonHangTheoTrangThaiAsync(string trangThai)
        {
            return await _dbSet
                .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(ct => ct.ChiTietSanPham)
                        .ThenInclude(sp => sp.SanPham)
                .Include(d => d.KhachHang)
                .Where(d => d.TrangThai == trangThai)
                .ToListAsync();
        }

        // Lấy toàn bộ đơn hàng
        public async Task<IEnumerable<DonHang>> LayTatCaDonHangAsync()
        {
            return await _dbSet
                .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(ct => ct.ChiTietSanPham)
                        .ThenInclude(sp => sp.SanPham)
                .Include(d => d.KhachHang)
                .ToListAsync();
        }

        // Lấy đơn hàng theo ID (bao gồm chi tiết và sản phẩm)
        public async Task<DonHang?> LayTheoIdAsync(int maDonHang)
        {
            return await _dbSet
                .Include(d => d.KhachHang)
                .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(ct => ct.ChiTietSanPham)
                        .ThenInclude(sp => sp.SanPham)
                .FirstOrDefaultAsync(d => d.MaDonHang == maDonHang);
        }
    }
}