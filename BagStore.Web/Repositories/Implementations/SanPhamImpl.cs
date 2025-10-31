using BagStore.Data;
using BagStore.Domain.Entities;
using BagStore.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BagStore.Web.Repositories.implementations
{
    public class SanPhamImpl : ISanPhamRepository
    {
        private readonly BagStoreDbContext _context;

        public SanPhamImpl(BagStoreDbContext context)
        {
            _context = context;
        }

        public async Task<SanPham> AddAsync(SanPham sanPham)
        {
            _context.SanPhams.Add(sanPham);
            await _context.SaveChangesAsync();
            return sanPham;
        }

        public async Task<bool> DeleteAsync(int maSP)
        {
            var sp = await _context.SanPhams.FindAsync(maSP);
            if (sp == null) return false;
            _context.SanPhams.Remove(sp);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<SanPham>> GetAllAsync()
        {
            return await _context.SanPhams
                .Include(p => p.AnhSanPhams)
                .ToListAsync();
        }

        public async Task<SanPham?> GetByIdAsync(int maSP)
        {
            return await _context.SanPhams
                .Include(p => p.AnhSanPhams)
                .FirstOrDefaultAsync(p => p.MaSP == maSP);
        }

        public async Task<SanPham> GetByNameAsync(string tenSanPham)
        {
            var entity = await _context.SanPhams.FirstOrDefaultAsync(x => x.TenSP == tenSanPham);
            return entity;
        }

        public async Task<SanPham> UpdateAsync(SanPham entity)
        {
            _context.SanPhams.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}