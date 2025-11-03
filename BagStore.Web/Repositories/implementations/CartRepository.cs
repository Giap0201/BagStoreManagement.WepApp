using BagStore.Data;
using BagStore.Domain.Entities;
using BagStore.Web.Models.DTOs.Responses;
using BagStore.Web.Models.Entities;
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

        public async Task<CartResponse> GetCartByUserIdAsync(string userId)
        {
            var khachHang = await _context.KhachHangs
       .FirstOrDefaultAsync(kh => kh.ApplicationUserId == userId);

            if (khachHang == null)
                return null;

            var items = await _context.GioHangs
                .Where(g => g.MaKH == khachHang.MaKH)
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
                    ThanhTien = g.SoLuong * g.ChiTietSanPham.GiaBan,
                    MaChiTietSP = g.MaChiTietSP
                })
                .ToListAsync();

            if (items == null || !items.Any())
                return null;

            return new CartResponse
            {
                MaKH = khachHang.MaKH,
                Items = items
            };
        }

        public async Task<GioHang?> GetCartItemAsync(int userId, int maCTSP)
        {
            return await _context.GioHangs
                .FirstOrDefaultAsync(g => g.MaKH == userId && g.MaChiTietSP == maCTSP);
        }
        //lay khach hang co userid theo tham so truyen vao
        public async Task<KhachHang?> GetCustomerByUserIdAsync(string userId)
        {
            return await  _context.KhachHangs
            .FirstOrDefaultAsync(kh => kh.ApplicationUserId == userId);
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

        public async Task<bool> ClearCartAsync()
        {
            var allItems = _context.GioHangs;
            _context.GioHangs.RemoveRange(allItems);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
