using BagStore.Web.Models.Entities;

namespace BagStore.Web.Repositories.Interfaces
{
    public interface IKhachHangRepository
    {
        Task<KhachHang?> GetByApplicationUserIdAsync(string applicationUserId);
    }
}
