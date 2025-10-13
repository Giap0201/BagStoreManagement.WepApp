using BagStore.Data;
using BagStore.Domain.Entities;
using BagStore.Web.Models.DTOs.SanPhams;
using BagStore.Web.Models.ViewModels.SanPhams;
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

        public async Task<SanPham?> GetByIdAsync(int maSP)
        {
            return await _context.SanPhams
                .Include(sp => sp.ChiTietSanPhams).ThenInclude(ct => ct.KichThuoc)
                .Include(sp => sp.ChiTietSanPhams).ThenInclude(ct => ct.MauSac)
                .Include(sp => sp.AnhSanPhams)
                .Include(sp => sp.DanhMucLoaiTui)
                .Include(sp => sp.ThuongHieu)
                .Include(sp => sp.ChatLieu)
                .FirstOrDefaultAsync(sp => sp.MaSP == maSP);
        }

        public async Task<SanPham> UpdateAsync(SanPham entity)
        {
            _context.SanPhams.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}