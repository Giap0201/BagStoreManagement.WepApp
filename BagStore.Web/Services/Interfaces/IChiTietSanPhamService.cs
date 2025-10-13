using BagStore.Domain.Entities;
using BagStore.Web.Models.DTOs.SanPhams;
using BagStore.Web.Models.ViewModels.SanPhams;

namespace BagStore.Web.Services.Interfaces
{
    public interface IChiTietSanPhamService
    {
        Task<ChiTietSanPhamResponseDto?> AddAsync(ChiTietSanPhamCreateDto dto);

        Task<ChiTietSanPhamResponseDto?> GetByIdAsync(int maChiTietSanPham);
    }
}