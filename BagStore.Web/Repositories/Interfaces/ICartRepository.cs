using BagStore.Web.Models.DTOs;
using BagStore.Web.Models.DTOs.Requests;
using BagStore.Web.Models.DTOs.Responses;

namespace BagStore.Web.Repositories.Interfaces
{
    public interface ICartRepository
    {
        CartResponse GetCartItems(int userId);
        Task<bool> AddSanPhamAsync(AddCartItemRequest request);

    }
}
