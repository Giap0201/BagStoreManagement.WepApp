using BagStore.Data;
using BagStore.Domain.Entities;
using BagStore.Web.Models.ViewModels;
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
                .Include(p => p.DanhMucLoaiTui)
                .Include(p => p.ThuongHieu)
                .Include(p => p.ChatLieu)
                .Include(p => p.AnhSanPhams)
                .ToListAsync();
        }

        public async Task<PageResult<SanPham>> GetAllPagingAsync(int page, int pageSize, string? search = null, int? maLoaiTui = null, int? maThuongHieu = null, int? maChatLieu = null)
        {
            //tao IQueryable co so voi cac include can thiet
            var query = _context.SanPhams
                .Include(p => p.DanhMucLoaiTui)
                .Include(p => p.ThuongHieu)
                .Include(p => p.ChatLieu)
                .Include(p => p.AnhSanPhams)
                .AsQueryable();

            //loc theo ten neu co search
            if (!string.IsNullOrEmpty(search))
            {
                var lowerSearch = search.ToLower();
                query = query.Where(p => p.TenSP.ToLower().Contains(lowerSearch));
            }

            //loc theo loai tui
            if (maLoaiTui.HasValue)
            {
                query = query.Where(p => p.MaLoaiTui == maLoaiTui.Value);
            }

            //loc theo thuong hieu
            if (maThuongHieu.HasValue)
            {
                query = query.Where(p => p.MaThuongHieu == maThuongHieu.Value);
            }

            //loc theo chat lieu
            if (maChatLieu.HasValue)
            {
                query = query.Where(p => p.MaChatLieu == maChatLieu.Value);
            }

            //lay tong so luong ban ghi (SAU khi filter)
            var totalRecords = await query.CountAsync();

            //ap dung phan trang
            var data = await query.OrderByDescending(p => p.NgayCapNhat).Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            //tra ve ket qua
            return new PageResult<SanPham>
            {
                Data = data,
                Total = totalRecords,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<SanPham?> GetByIdAsync(int maSP)
        {
            return await _context.SanPhams
                .Include(p => p.DanhMucLoaiTui)
                .Include(p => p.ThuongHieu)
                .Include(p => p.ChatLieu)
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