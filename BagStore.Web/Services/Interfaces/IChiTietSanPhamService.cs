using BagStore.Models.Common;
using BagStore.Web.Models.DTOs.Requests;
using BagStore.Web.Models.ViewModels;

namespace BagStore.Web.Services.Interfaces
{
    public interface IChiTietSanPhamService
    {
        Task<BaseResponse<ChiTietSanPhamResponseDto>> CreateAsync(ChiTietSanPhamRequestDto dto);

        Task<BaseResponse<ChiTietSanPhamResponseDto>> UpdateAsync(int maChiTietSP, ChiTietSanPhamRequestDto dto);

        Task<BaseResponse<bool>> DeleteAsync(int maChiTietSP);

        Task<BaseResponse<ChiTietSanPhamResponseDto>> GetByIdAsync(int maChiTietSP);

        Task<BaseResponse<List<ChiTietSanPhamResponseDto>>> GetBySanPhamIdAsync(int maSP);

        Task<BaseResponse<List<ChiTietSanPhamResponseDto>>> GetAllAsync();
    }
}