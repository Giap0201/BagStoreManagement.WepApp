using BagStore.Domain.Entities;
using BagStore.Web.Models.DTOs.SanPhams;
using BagStore.Web.Models.ViewModels.SanPhams;

namespace BagStore.Web.Services.Interfaces
{
    public interface IChiTietSanPhamService
    {
        Task<ChiTietSanPhamResponseDto?> AddAsync(int MaSP, ChiTietSanPhamCreateDto dto);

        Task<ChiTietSanPhamResponseDto?> GetByIdAsync(int maChiTietSanPham);

        Task<ChiTietSanPhamResponseDto> UpdateAsync(int maChiTietSanPham, ChiTietSanPhamCreateDto dto);

        Task<bool> DeleteAsync(int maChiTietSanPham);

        // Lay danh sach bien the cua mot san pham
        Task<List<ChiTietSanPham>> GetBySanPhamIdAsync(int maSP);
    }
}