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

        public async Task<ChiTietSanPham> AddAsync(ChiTietSanPham entity)
        {
            _context.ChiTietSanPhams.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int maChiTietSanPham)
        {
            var ct = await _context.ChiTietSanPhams.FindAsync(maChiTietSanPham);
            if (ct == null) return false;
            _context.ChiTietSanPhams.Remove(ct);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ChiTietSanPham?> FindByAttributesAsync(int maSP, int maMauSac, int maKichThuoc)
        {
            return await _context.ChiTietSanPhams
                .FirstOrDefaultAsync(x => x.MaSP == maSP
                    && x.MaMauSac == maMauSac && x.MaKichThuoc == maKichThuoc
            );
        }

        public async Task<ChiTietSanPham?> GetByIdAsync(int maChiTietSanPham)
        {
            return await _context.ChiTietSanPhams
                .Include(ct => ct.KichThuoc)
                .Include(ct => ct.MauSac)
                .FirstOrDefaultAsync(ct => ct.MaChiTietSP == maChiTietSanPham);
        }

        public async Task<List<ChiTietSanPham>> GetBySanPhamIdAsync(int maSP)
        {
            return await _context.ChiTietSanPhams.Where(x => x.MaSP == maSP).ToListAsync();
        }

        public async Task<ChiTietSanPham> UpdateAsync(ChiTietSanPham entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}