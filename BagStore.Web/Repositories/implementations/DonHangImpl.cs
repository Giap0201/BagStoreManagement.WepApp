using BagStore.Data;
using BagStore.Domain.Entities;
using BagStore.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BagStore.Web.Repositories.implementations
{
    public class DonHangImpl : IDonHangRepository
    {
        private readonly BagStoreDbContext _context;

        public DonHangImpl(BagStoreDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DonHang>> LayTatCaDonHangAsync()
        {
            return await _context.DonHangs
                .Include(d => d.KhachHang)
                .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(ct => ct.ChiTietSanPham)
                        .ThenInclude(ctsp => ctsp.SanPham)
                            .ThenInclude(sp => sp.AnhSanPhams)
                .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(ct => ct.ChiTietSanPham)
                        .ThenInclude(ctsp => ctsp.MauSac)
                .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(ct => ct.ChiTietSanPham)
                        .ThenInclude(ctsp => ctsp.KichThuoc)
                .OrderByDescending(d => d.NgayDatHang)
                .ToListAsync();
        }

        public async Task<IEnumerable<DonHang>> LayDonHangTheoUserAsync(string userId)
        {
            return await _context.DonHangs
                .Include(d => d.KhachHang)
                .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(ct => ct.ChiTietSanPham)
                        .ThenInclude(ctsp => ctsp.SanPham)
                            .ThenInclude(sp => sp.AnhSanPhams)
                .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(ct => ct.ChiTietSanPham)
                        .ThenInclude(ctsp => ctsp.MauSac)
                .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(ct => ct.ChiTietSanPham)
                        .ThenInclude(ctsp => ctsp.KichThuoc)
                .Where(d => d.KhachHang.ApplicationUserId == userId)
                .OrderByDescending(d => d.NgayDatHang)
                .ToListAsync();
        }

        public async Task<IEnumerable<DonHang>> LayDonHangTheoKhachHangAsync(int maKhachHang)
        {
            return await _context.DonHangs
                .Include(d => d.KhachHang)
                .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(ct => ct.ChiTietSanPham)
                        .ThenInclude(ctsp => ctsp.SanPham)
                            .ThenInclude(sp => sp.AnhSanPhams)
                .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(ct => ct.ChiTietSanPham)
                        .ThenInclude(ctsp => ctsp.MauSac)
                .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(ct => ct.ChiTietSanPham)
                        .ThenInclude(ctsp => ctsp.KichThuoc)
                .Where(d => d.MaKH == maKhachHang)
                .OrderByDescending(d => d.NgayDatHang)
                .ToListAsync();
        }

        public async Task<DonHang?> LayTheoIdAsync(int maDonHang)
        {
            return await _context.DonHangs
                .FirstOrDefaultAsync(d => d.MaDonHang == maDonHang);
        }

        public async Task<DonHang?> GetByIdWithDetailsAsync(int maDonHang)
        {
            return await _context.DonHangs
                .Include(d => d.KhachHang)
                .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(ct => ct.ChiTietSanPham)
                        .ThenInclude(ctsp => ctsp.SanPham)
                            .ThenInclude(sp => sp.AnhSanPhams)
                .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(ct => ct.ChiTietSanPham)
                        .ThenInclude(ctsp => ctsp.MauSac)
                .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(ct => ct.ChiTietSanPham)
                        .ThenInclude(ctsp => ctsp.KichThuoc)
                .FirstOrDefaultAsync(d => d.MaDonHang == maDonHang);
        }

        public async Task AddAsync(DonHang entity)
        {
            await _context.DonHangs.AddAsync(entity);
        }

        public async Task UpdateAsync(DonHang entity)
        {
            _context.DonHangs.Update(entity);
            await _context.SaveChangesAsync();
        }

        // Cùng chức năng với UpdateAsync – tùy bạn muốn dùng tên nào
        public async Task CapNhatAsync(DonHang entity)
        {
            _context.DonHangs.Update(entity);
            await _context.SaveChangesAsync();
        }

        // Duplicate SaveAsync để tương thích với LuuAsync trong interface
        public async Task LuuAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task XoaAsync(int maDonHang)
        {
            var donHang = await _context.DonHangs
                .Include(d => d.ChiTietDonHangs)
                .FirstOrDefaultAsync(d => d.MaDonHang == maDonHang);

            if (donHang == null)
                return;

            // Nếu chưa bật cascade delete, ta cần xóa chi tiết trước
            if (donHang.ChiTietDonHangs != null && donHang.ChiTietDonHangs.Any())
            {
                _context.ChiTietDonHangs.RemoveRange(donHang.ChiTietDonHangs);
            }

            _context.DonHangs.Remove(donHang);
            await _context.SaveChangesAsync();
        }
    }
}