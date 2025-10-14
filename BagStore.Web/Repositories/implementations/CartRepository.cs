using BagStore.Data;
using BagStore.Domain.Entities;
using BagStore.Web.Models.DTOs.Responses;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BagStore.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly BagStoreDbContext _context;

        public CartRepository(BagStoreDbContext context)
        {
            _context = context;
        }

        public async Task<CartResponse> GetCartByUserIdAsync(int userId)
        {
            var items = await _context.GioHangs
                .Where(g => g.MaKH == userId)
                .Select(g => new CartItemResponse
                {
                    MaSP_GH = g.MaSP_GH,
                    TenSP = g.ChiTietSanPham.SanPham.TenSP,
                    GiaBan = g.ChiTietSanPham.GiaBan,
                    SoLuong = g.SoLuong,
                    MauSac = g.ChiTietSanPham.MauSac.TenMauSac,
                    KichThuoc = g.ChiTietSanPham.KichThuoc.TenKichThuoc,
                    DuongDanAnh = g.ChiTietSanPham.SanPham.AnhSanPhams
                        .Select(a => a.DuongDan)
                        .FirstOrDefault() ?? "/images/no-image.png",
                    ThanhTien = g.SoLuong * g.ChiTietSanPham.GiaBan
                })
                .ToListAsync();

            if (items == null || !items.Any())
                return null;

            return new CartResponse
            {
                MaKH = userId,
                Items = items
            };
        }

        public async Task<GioHang> GetCartItemAsync(int userId, int maCTSP)
        {
            return await _context.GioHangs
                .FirstOrDefaultAsync(g => g.MaKH == userId && g.MaChiTietSP == maCTSP);
        }

        public async Task AddCartItemAsync(GioHang item)
        {
            await _context.GioHangs.AddAsync(item);
        }

        public async Task UpdateCartItemAsync(GioHang item)
        {
            _context.GioHangs.Update(item);
            await Task.CompletedTask;
        }

        public async Task RemoveCartItemAsync(GioHang item)
        {
            _context.GioHangs.Remove(item);
            await Task.CompletedTask;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
}
}
