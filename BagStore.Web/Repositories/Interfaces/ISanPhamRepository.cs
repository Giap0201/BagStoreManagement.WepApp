using BagStore.Domain.Entities;
using BagStore.Web.Models.DTOs.SanPhams;
using BagStore.Web.Models.ViewModels.SanPhams;

namespace BagStore.Web.Repositories.Interfaces
{
    public interface ISanPhamRepository
    {
        Task<SanPham> AddSanPhamAsync(SanPham sanPham);

        Task<SanPham?> GetSanPhamByIdAsync(int maSP);
    }
}