
using BagStore.Domain.Entities;
using BagStore.Repositories;
using BagStore.Web.Models.DTOs.Requests;
using BagStore.Web.Models.DTOs.Responses;
using System.Linq;
using System.Threading.Tasks;

namespace BagStore.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        //  Lấy giỏ hàng theo user
        public async Task<CartResponse> GetCartByUserIdAsync(string userId)
        {
            return await _cartRepository.GetCartByUserIdAsync(userId);
        }

        //  Thêm sản phẩm vào giỏ
        public async Task<bool> AddToCartAsync(AddCartItemRequest request)
        {
            // Kiểm tra xem sản phẩm đã tồn tại trong giỏ hàng chưa
            var existingItem = await _cartRepository.GetCartItemAsync(request.MaKH, request.MaChiTietSP);

            if (existingItem != null)
            {
                // Nếu đã có thì tăng số lượng
                existingItem.SoLuong += request.SoLuong;
                await _cartRepository.UpdateCartItemAsync(existingItem);
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

                await _cartRepository.AddCartItemAsync(newItem);
            }
            return await _cartRepository.SaveChangesAsync() > 0;
        }


        //  Xóa sản phẩm khỏi giỏ
        public async Task<bool> RemoveCartItemAsync(int MaKH, int MaChiTietSP)
        {
            var item = await _cartRepository.GetCartItemAsync(MaKH,MaChiTietSP);
            if (item == null) return false;

            await _cartRepository.RemoveCartItemAsync(item);
            await _cartRepository.SaveChangesAsync();
            return true;
        }

        //  Xóa toàn bộ giỏ hàng
        //public async Task<bool> ClearCartAsync(int userId)
        //{
        //    var cart = await _cartRepository.GetCartByUserIdAsync(userId);
        //    if (cart == null || !cart.Items.Any())
        //        return false;

        //    foreach (var item in cart.Items)
        //    {
        //        var entity = await _cartRepository.GetCartItemAsync(userId, item.MaSP_GH);
        //        if (entity != null)
        //            await _cartRepository.RemoveCartItemAsync(entity);
        //    }

        //    await _cartRepository.SaveChangesAsync();
        //    return true;
        //}
    }
}
