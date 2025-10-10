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

        public async Task<int> CreateAsync(ThuongHieuDto request)
        {
            var entity = new ThuongHieu
            {
                TenThuongHieu = request.TenThuongHieu,
                QuocGia = request.QuocGia
            };
            _context.ThuongHieux.Add(entity);
            await _context.SaveChangesAsync();
            return entity.MaThuongHieu;
        }

        public async Task<bool> DeleteAsync(int maThuongHieu)
        {
            var entity = await _context.ThuongHieux.FindAsync(maThuongHieu);
            if (entity == null) return false;
            _context.ThuongHieux.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ThuongHieuDto>> GetAllAsync()
        {
            return await _context.ThuongHieux.Select(x => new ThuongHieuDto
            {
                MaThuongHieu = x.MaThuongHieu,
                TenThuongHieu = x.TenThuongHieu,
                QuocGia = x.QuocGia
            }).ToListAsync();
        }

        public async Task<ThuongHieuDto> GetByIdAsync(int maThuongHieu)
        {
            var entity = await _context.ThuongHieux.FindAsync(maThuongHieu);
            if (entity == null) return null;
            return new ThuongHieuDto
            {
                MaThuongHieu = entity.MaThuongHieu,
                TenThuongHieu = entity.TenThuongHieu,
                QuocGia = entity.QuocGia
            };
        }

        public async Task<bool> UpdateAsync(ThuongHieuDto request)
        {
            var entity = await _context.ThuongHieux.FindAsync(request.MaThuongHieu);
            if (entity == null) return false;
            entity.TenThuongHieu = request.TenThuongHieu;
            entity.QuocGia = request.QuocGia;
            _context.ThuongHieux.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}