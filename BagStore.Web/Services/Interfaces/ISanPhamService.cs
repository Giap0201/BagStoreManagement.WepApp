using BagStore.Domain.Entities;
using BagStore.Web.Models.DTOs.SanPhams;
using BagStore.Web.Models.ViewModels.SanPhams;
using BagStore.Web.Utilities;

namespace BagStore.Web.Services.Interfaces
{
    public interface ISanPhamService
    {
        Task<SanPhamDetailResponseDto> CreateAsync(SanPhamCreateDto dto);

        Task<SanPhamDetailResponseDto> GetByIdAsync(int maSP);

        Task<SanPhamDetailResponseDto> UpdateAsync(int maSP, SanPhamUpdateDto dto);

        Task<bool> DeleteAsync(int maSP);
    }
}