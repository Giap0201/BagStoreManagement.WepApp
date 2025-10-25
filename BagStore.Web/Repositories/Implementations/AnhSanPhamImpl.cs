using BagStore.Data;
using BagStore.Domain.Entities;
using BagStore.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BagStore.Web.Repositories.Implementations
{
    public class AnhSanPhamImpl : IAnhSanPhamRepository
    {
        private readonly BagStoreDbContext _context;

        public AnhSanPhamImpl(BagStoreDbContext context)
        {
            _context = context;
        }

        public async Task<AnhSanPham> AddAsync(AnhSanPham anhSanPham)
        {
            await _context.AnhSanPhams.AddAsync(anhSanPham);
            await _context.SaveChangesAsync();
            return anhSanPham;
        }

        public async Task<bool> DeleteAsync(int maAnh)
        {
            var entity = await _context.AnhSanPhams.FindAsync(maAnh);
            if (entity == null) return false;
            _context.AnhSanPhams.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<AnhSanPham?> GetByIdAsync(int maAnh)
        {
            return await _context.AnhSanPhams.FirstOrDefaultAsync(x => x.MaAnh == maAnh);
        }

        public async Task<List<AnhSanPham>> GetBySanPhamIdAsync(int maSP)
        {
            return await _context.AnhSanPhams.Where(a => a.MaSP == maSP)
                .OrderByDescending(a => a.LaHinhChinh)
                .ThenBy(a => a.MaAnh)
                .ToListAsync();
        }

        public Task<bool> SetMainImageAsync(int maSP, int maAnh)
        {
            throw new NotImplementedException();
        }

        public async Task<AnhSanPham> UpdateAsync(AnhSanPham entity)
        {
            _context.AnhSanPhams.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}