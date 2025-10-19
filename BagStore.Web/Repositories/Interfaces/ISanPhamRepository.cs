using BagStore.Domain.Entities;
using BagStore.Web.Models.DTOs.SanPhams;
using BagStore.Web.Models.ViewModels.SanPhams;

namespace BagStore.Web.Repositories.Interfaces
{
    public interface ISanPhamRepository
    {
        Task<SanPham> AddAsync(SanPham sanPham);

        Task<SanPham?> GetByIdAsync(int maSP);

        Task<SanPham> UpdateAsync(SanPham entity);

        Task<bool> DeleteAsync(int maSP);
    }
}