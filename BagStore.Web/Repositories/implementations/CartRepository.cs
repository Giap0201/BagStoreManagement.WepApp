using BagStore.Data;
using BagStore.Web.Models.DTOs;
using BagStore.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BagStore.Web.Repositories.Implementations
{
    public class CartRepository : ICartRepository
    {
        private readonly BagStoreDbContext _context;

        public CartRepository(BagStoreDbContext context)
        {
            _context = context;
        }

        public List<CartItemDTO> GetCartItems(int userId)
        {
            
            var khachHang = _context.KhachHangs.FirstOrDefault(k => k.MaKH == userId);
            if (khachHang == null)
            {
                throw new Exception("Người dùng không tồn tại");
            }

            
            var hasCart = _context.GioHangs.Any(g => g.MaKH == userId);
            if (!hasCart)
            {
                throw new Exception("Giỏ hàng trống");
            }

            var query = _context.GioHangs
                .Where(g => g.MaKH == userId)
                .Join(_context.ChiTietSanPhams, g => g.MaChiTietSP, ctsp => ctsp.MaChiTietSP, (g, ctsp) => new { g, ctsp })
                .Join(_context.SanPhams, x => x.ctsp.MaSP, sp => sp.MaSP, (x, sp) => new { x.g, x.ctsp, sp })
                .Join(_context.MauSacs, x => x.ctsp.MaMauSac, m => m.MaMauSac, (x, m) => new { x.g, x.ctsp, x.sp, m })
                .Join(_context.KichThuocs, x => x.ctsp.MaKichThuoc, k => k.MaKichThuoc, (x, k) => new { x.g, x.ctsp, x.sp, x.m, k })
                .GroupJoin(_context.AnhSanPhams, x => x.sp.MaSP, a => a.MaSP, (x, anhGroup) => new { x.g, x.ctsp, x.sp, x.m, x.k, anhGroup })
                .SelectMany(x => x.anhGroup.DefaultIfEmpty(), (x, anh) => new CartItemDTO
                {
                    MaGioHang = x.g.MaGioHang,
                    TenSP = x.sp.TenSP,
                    GiaBan = x.ctsp.GiaBan,
                    SoLuong = x.g.SoLuong,
                    MauSac = x.m.TenMauSac,
                    KichThuoc = x.k.TenKichThuoc,
                    DuongDanAnh = anh != null ? anh.DuongDan : "/images/no-image.png",
                    ThanhTien = x.g.SoLuong * x.ctsp.GiaBan
                })
                .ToList();

            return query;
        }
    }
}
