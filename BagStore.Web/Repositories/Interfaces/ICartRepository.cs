using BagStore.Web.Models.DTOs;

namespace BagStore.Web.Repositories.Interfaces
{
    public interface ICartRepository
    {
        List<CartItemDTO> GetCartItems(int userId);
    }
}
