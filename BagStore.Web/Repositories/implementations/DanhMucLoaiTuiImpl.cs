using BagStore.Data;
using BagStore.Domain.Entities;
using BagStore.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BagStore.Web.Repositories.Implementations
{
    public class DanhMucLoaiTuiImpl : IDanhMucLoaiTuiRepository
    {
        private readonly BagStoreDbContext _context;

        public DanhMucLoaiTuiImpl(BagStoreDbContext context)
        {
            _context = context;
        }

        // Thêm mới loại túi
        public async Task<DanhMucLoaiTui> AddAsync(DanhMucLoaiTui entity)
        {
            _context.DanhMucLoaiTuis.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        // Xóa loại túi
        public async Task<bool> DeleteAsync(int maLoaiTui)
        {
            var entity = await _context.DanhMucLoaiTuis.FindAsync(maLoaiTui);
            if (entity == null) return false;

            _context.DanhMucLoaiTuis.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        // Lấy tất cả loại túi
        public async Task<List<DanhMucLoaiTui>> GetAllAsync()
        {
            return await _context.DanhMucLoaiTuis
                         .AsNoTracking()
                         .ToListAsync();
        }

        // Lấy loại túi theo ID
        public async Task<DanhMucLoaiTui> GetByIdAsync(int maLoaiTui)
        {
            return await _context.DanhMucLoaiTuis.FindAsync(maLoaiTui);
        }

        public async Task<DanhMucLoaiTui> GetByNameAsync(string tenLoaiTui)
        {
            var entity = await _context.DanhMucLoaiTuis
                         .FirstOrDefaultAsync(x => x.TenLoaiTui == tenLoaiTui);
            if (entity == null) return null;
            return entity;
        }

        // Cập nhật loại túi
        public async Task<DanhMucLoaiTui> UpdateAsync(DanhMucLoaiTui entity)
        {
            _context.DanhMucLoaiTuis.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}