using BagStore.Data;
using BagStore.Domain.Entities;
using BagStore.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BagStore.Web.Repositories.implementations
{
    public class DonHangImpl : GenericImpl<DonHang>, IDonHangRepository
    {
        public DonHangImpl(BagStoreDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<DonHang>> LayDonHangTheoKhachHangAsync(int maKH)
        {
            return await _dbSet
                .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(ct => ct.ChiTietSanPham)
                        .ThenInclude(c => c.SanPham) // thêm dòng này
                .Where(d => d.MaKH == maKH)
                .ToListAsync();
        }

        public async Task<IEnumerable<DonHang>> LayDonHangTheoTrangThaiAsync(string trangThai)
        {
            return await _dbSet
                .Include(d => d.ChiTietDonHangs)
                .ThenInclude(ct => ct.ChiTietSanPham)
                .Where(d => d.TrangThai == trangThai)
                .ToListAsync();
        }

        // Triển khai lấy tất cả đơn hàng
        public async Task<IEnumerable<DonHang>> LayTatCaDonHangAsync()
        {
            return await _dbSet
                .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(ct => ct.ChiTietSanPham)
                .Include(d => d.KhachHang)  // để Admin xem tên khách hàng
                .ToListAsync();
        }
    }
}
