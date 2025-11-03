using BagStore.Data;
using BagStore.Domain.Entities;
using BagStore.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BagStore.Web.Repositories.implementations
{
    public class ChiTietDonHangImpl : IChiTietDonHangRepository
    {
        private readonly BagStoreDbContext _context;

        public ChiTietDonHangImpl(BagStoreDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ChiTietDonHang entity)
        {
            await _context.ChiTietDonHangs.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ChiTietDonHang>> LayTheoDonHangAsync(int maDonHang)
        {
            return await _context.ChiTietDonHangs
                .Include(ct => ct.ChiTietSanPham)
                    .ThenInclude(sp => sp.SanPham)// load sản phẩm (nếu có navigation)
                        .ThenInclude(s => s.AnhSanPhams) // load ảnh sản phẩm
                .Where(ct => ct.MaDonHang == maDonHang)
                .ToListAsync();
        }
    }
}
