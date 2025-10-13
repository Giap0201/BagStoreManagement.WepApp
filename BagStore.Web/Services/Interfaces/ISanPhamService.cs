using BagStore.Domain.Entities;
using BagStore.Web.Models.DTOs.SanPhams;
using BagStore.Web.Models.ViewModels.SanPhams;
using BagStore.Web.Utilities;

namespace BagStore.Web.Services.Interfaces
{
    public interface ISanPhamService
    {
        Task<SanPhamDetailResponseDto> CreateSanPhamAsync(SanPhamCreateDto dto);

        Task<SanPhamDetailResponseDto> GetSanPhamByIdAsync(int maSP);
    }
}