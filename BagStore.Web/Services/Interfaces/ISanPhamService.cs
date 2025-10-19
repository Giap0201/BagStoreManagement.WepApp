using BagStore.Domain.Entities;
using BagStore.Models.Common;
using BagStore.Web.Models.DTOs.SanPhams;
using BagStore.Web.Models.ViewModels.SanPhams;
using BagStore.Web.Utilities;

namespace BagStore.Web.Services.Interfaces
{
    public interface ISanPhamService
    {
        Task<BaseResponse<SanPhamResponseDto>> CreateAsync(SanPhamCreateDto dto);

        Task<BaseResponse<SanPhamResponseDto>> GetByIdAsync(int maSanPham);

        Task<BaseResponse<List<SanPhamResponseDto>>> GetAllAsync();

        Task<BaseResponse<SanPhamResponseDto>> UpdateAsync(int maSanPham, SanPhamUpdateDto dto);

        Task<BaseResponse<bool>> DeleteAsync(int maSanPham);
    }
}