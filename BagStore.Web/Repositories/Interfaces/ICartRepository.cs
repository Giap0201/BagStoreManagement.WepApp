
using BagStore.Domain.Entities;
using BagStore.Web.Models.DTOs.Responses;
using BagStore.Web.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BagStore.Repositories
{
    public interface ICartRepository
    {
        // Lấy danh sách sản phẩm trong giỏ hàng theo userId
        Task<CartResponse> GetCartByUserIdAsync(string userId);

        // Lấy 1 item trong giỏ hàng theo userId và MaCTSP
        Task<GioHang> GetCartItemAsync(int userId, int maCTSP);

        // Thêm 1 sản phẩm vào giỏ hàng
        Task AddCartItemAsync(GioHang item);

        // Cập nhật sản phẩm trong giỏ hàng (VD: tăng số lượng)
        Task UpdateCartItemAsync(GioHang item);

        // Xóa sản phẩm khỏi giỏ
        Task RemoveCartItemAsync(GioHang item);

        // Lưu thay đổi
        Task<int> SaveChangesAsync();

        Task<bool> ClearCartAsync();

       Task<KhachHang?> GetCustomerByUserIdAsync(string userId);
    }
}
