using BagStore.Data;
using BagStore.Domain.Entities;
using BagStore.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

public class ChiTietSanPhamImpl : IChiTietSanPhamRepository
{
    private readonly BagStoreDbContext _context;

    public ChiTietSanPhamImpl(BagStoreDbContext context)
    {
        _context = context;
    }

    /// Thêm chi tiết sản phẩm mới vào DB

    public async Task<ChiTietSanPham> AddAsync(ChiTietSanPham chiTiet)
    {
        _context.ChiTietSanPhams.Add(chiTiet);
        await _context.SaveChangesAsync();
        return chiTiet;
    }

    /// Xóa chi tiết sản phẩm theo ID, trả về true nếu xóa thành công

    public async Task<bool> DeleteAsync(int maChiTietSP)
    {
        var entity = await _context.ChiTietSanPhams.FindAsync(maChiTietSP);
        if (entity == null) return false;

        _context.ChiTietSanPhams.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    /// Lấy tất cả chi tiết sản phẩm, include Kích thước và Màu sắc

    public async Task<List<ChiTietSanPham>> GetAllAsync()
    {
        return await _context.ChiTietSanPhams
            .Include(ct => ct.KichThuoc)
            .Include(ct => ct.MauSac)
            .ToListAsync();
    }

        public async Task<ChiTietSanPham?> GetByIdAsync(int maChiTietSanPham)
        {
            return await _context.ChiTietSanPhams
                .Include(ct => ct.SanPham)
                .Include(ct => ct.KichThuoc)
                .Include(ct => ct.MauSac)
                .FirstOrDefaultAsync(ct => ct.MaChiTietSP == maChiTietSanPham);
        }

    /// Lấy danh sách chi tiết sản phẩm theo ID sản phẩm

    public async Task<List<ChiTietSanPham>> GetBySanPhamIdAsync(int maSP)
    {
        return await _context.ChiTietSanPhams
            .Include(ct => ct.KichThuoc)
            .Include(ct => ct.MauSac)
            .Where(ct => ct.MaSP == maSP)
            .ToListAsync();
    }

    /// Cập nhật chi tiết sản phẩm

    public async Task<ChiTietSanPham> UpdateAsync(ChiTietSanPham chiTiet)
    {
        _context.ChiTietSanPhams.Update(chiTiet);
        await _context.SaveChangesAsync();
        return chiTiet;
    }
}