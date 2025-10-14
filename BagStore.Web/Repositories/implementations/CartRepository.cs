using BagStore.Data;
using BagStore.Domain.Entities;
using BagStore.Web.Models.DTOs;
using BagStore.Web.Models.DTOs.Requests;
using BagStore.Web.Models.DTOs.Responses;
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

        public CartResponse GetCartItems(int userId)
        {

            var items = _context.GioHangs
                .Where(g => g.MaKH == userId)
                .Join(_context.ChiTietSanPhams, g => g.MaChiTietSP, ctsp => ctsp.MaChiTietSP, (g, ctsp) => new { g, ctsp })
                .Join(_context.SanPhams, x => x.ctsp.MaSP, sp => sp.MaSP, (x, sp) => new { x.g, x.ctsp, sp })
                .Join(_context.MauSacs, x => x.ctsp.MaMauSac, m => m.MaMauSac, (x, m) => new { x.g, x.ctsp, x.sp, m })
                .Join(_context.KichThuocs, x => x.ctsp.MaKichThuoc, k => k.MaKichThuoc, (x, k) => new { x.g, x.ctsp, x.sp, x.m, k })
                .GroupJoin(_context.AnhSanPhams, x => x.sp.MaSP, a => a.MaSP, (x, anhGroup) => new { x.g, x.ctsp, x.sp, x.m, x.k, anhGroup })
                .SelectMany(x => x.anhGroup.DefaultIfEmpty(), (x, anh) => new CartItemResponse
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

            return new CartResponse
            {
                UserId = userId,
                Items = items
            };
        }
        public async Task<bool> AddSanPhamAsync(AddCartItemRequest request)
        {
  
            var spTrongGio = await _context.GioHangs
                .FirstOrDefaultAsync(c => c.MaKH == request.MaKH 
                && c.MaChiTietSP == request.MaChiTietSP);

            if (spTrongGio != null)
            {
                // Nếu có rồi thì tăng số lượng
                spTrongGio.SoLuong += request.SoLuong;
            }
            else
            {
                // Nếu chưa có thì thêm mới
                var newItem = new GioHang
                {
                    MaKH = request.MaKH,
                    MaChiTietSP = request.MaChiTietSP,
                    SoLuong = request.SoLuong
                };
                _context.GioHangs.Add(newItem);
            }

            await _context.SaveChangesAsync();
            return true;
        }


    }
}
