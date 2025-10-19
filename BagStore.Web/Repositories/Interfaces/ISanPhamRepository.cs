using BagStore.Domain.Entities;

namespace BagStore.Web.Repositories.Interfaces
{
    public interface ISanPhamRepository
    {
        Task<SanPham> AddAsync(SanPham sanPham);

        Task<SanPham?> GetByIdAsync(int maSP);

        Task<SanPham> UpdateAsync(SanPham entity);

        Task<bool> DeleteAsync(int maSP);

        Task<List<SanPham>> GetAllAsync();

        Task<SanPham> GetByNameAsync(string tenSanPham);
    }
}