using BagStore.Domain.Entities;

namespace BagStore.Web.Repositories.Interfaces
{
    public interface IChiTietSanPhamRepository
    {
        Task<ChiTietSanPham> AddAsync(ChiTietSanPham chiTiet);

        Task<ChiTietSanPham> UpdateAsync(ChiTietSanPham chiTiet);

        Task<bool> DeleteAsync(int maChiTietSP);

        Task<ChiTietSanPham?> GetByIdAsync(int maChiTietSP);

        Task<List<ChiTietSanPham>> GetAllAsync();

        Task<List<ChiTietSanPham>> GetBySanPhamIdAsync(int maSP);
    }
}