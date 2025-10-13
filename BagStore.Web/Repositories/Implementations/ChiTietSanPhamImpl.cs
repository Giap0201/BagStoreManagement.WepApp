using BagStore.Data;
using BagStore.Domain.Entities;
using BagStore.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BagStore.Web.Repositories.Implementations
{
    public class ChiTietSanPhamImpl : IChiTietSanPhamRepository
    {
        private readonly BagStoreDbContext _context;

        public ChiTietSanPhamImpl(BagStoreDbContext context)
        {
            _context = context;
        }

        public async Task<ChiTietSanPham> AddAsync(ChiTietSanPham chiTiet)
        {
            _context.ChiTietSanPhams.Add(chiTiet);
            await _context.SaveChangesAsync();
            return chiTiet;
        }

        public async Task<ChiTietSanPham?> GetByIdAsync(int maChiTietSanPham)
        {
            return await _context.ChiTietSanPhams.Include(ct => ct.KichThuoc)
                .Include(ct => ct.MauSac).FirstOrDefaultAsync(ct => ct.MaChiTietSP == maChiTietSanPham);
        }
    }
}