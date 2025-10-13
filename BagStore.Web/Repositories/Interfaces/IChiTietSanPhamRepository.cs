using BagStore.Domain.Entities;

namespace BagStore.Web.Repositories.Interfaces
{
    public interface IChiTietSanPhamRepository
    {
        Task<ChiTietSanPham?> GetByIdAsync(int maChiTietSanPham);

        Task<ChiTietSanPham> AddAsync(ChiTietSanPham chiTiet);
    }
}