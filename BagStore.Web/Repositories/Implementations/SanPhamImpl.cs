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

        public async Task<SanPham> AddSanPhamAsync(SanPham sanPham)
        {
            _context.SanPhams.Add(sanPham);
            await _context.SaveChangesAsync();
            return sanPham;
        }

        public async Task<SanPham?> GetSanPhamByIdAsync(int maSP)
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
    }
}