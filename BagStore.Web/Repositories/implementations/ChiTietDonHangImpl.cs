using BagStore.Data;
using BagStore.Domain.Entities;
using BagStore.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BagStore.Web.Repositories.implementations
{
    public class ChiTietDonHangImpl : GenericImpl<ChiTietDonHang>, IChiTietDonHangRepository
    {
        public ChiTietDonHangImpl(BagStoreDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ChiTietDonHang>> LayTheoDonHangAsync(int maDonHang)
        {
            return await _dbSet
                .Include(ct => ct.ChiTietSanPham)
                .Where(ct => ct.MaDonHang == maDonHang)
                .ToListAsync();
        }
    }
}
