using BagStore.Data;
using BagStore.Domain.Entities;
using BagStore.Web.Models.DTOs;
using BagStore.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BagStore.Web.Repositories.implementations
{
    public class ThuongHieuImpl : IThuongHieuRepository
    {
        private readonly BagStoreDbContext _context;

        public ThuongHieuImpl(BagStoreDbContext context)
        {
            _context = context;
        }

        // Thêm mới thương hiệu
        public async Task<ThuongHieu> AddAsync(ThuongHieu entity)
        {
            _context.ThuongHieux.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        // Xóa thương hiệu
        public async Task<bool> DeleteAsync(int maThuongHieu)
        {
            var entity = await _context.ThuongHieux.FindAsync(maThuongHieu);
            if (entity == null) return false;

            _context.ThuongHieux.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        // Lấy tất cả thương hiệu
        public async Task<List<ThuongHieu>> GetAllAsync()
        {
            return await _context.ThuongHieux.ToListAsync();
        }

        // Lấy thương hiệu theo ID
        public async Task<ThuongHieu> GetByIdAsync(int maThuongHieu)
        {
            return await _context.ThuongHieux.FindAsync(maThuongHieu);
        }

        public async Task<ThuongHieu> GetByNameAsync(string tenThuongHieu)
        {
            var entity = await _context.ThuongHieux.FirstOrDefaultAsync(x => x.TenThuongHieu == tenThuongHieu);
            if (entity == null) return null;
            return entity;
        }

        // Cập nhật thương hiệu
        public async Task<ThuongHieu> UpdateAsync(ThuongHieu entity)
        {
            _context.ThuongHieux.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}