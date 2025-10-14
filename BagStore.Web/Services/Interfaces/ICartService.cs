
using BagStore.Web.Models.DTOs.Requests;
using BagStore.Web.Models.DTOs.Responses;
using System.Threading.Tasks;

namespace BagStore.Services
{
    public interface ICartService
    {
        // Lấy danh sách giỏ hàng của người dùng
        Task<CartResponse> GetCartByUserIdAsync(int userId);

        // Thêm sản phẩm vào giỏ
        Task<bool> AddToCartAsync(AddCartItemRequest request);

        // Xóa sản phẩm khỏi giỏ
        Task<bool> RemoveCartItemAsync(RemoveCartItemRequest request);

        // Xóa toàn bộ giỏ hàng (nếu cần)
        Task<bool> ClearCartAsync(int userId);
    }
}
