using BagStore.Domain.Entities;
using BagStore.Models.Common;
using BagStore.Web.Models.DTOs.SanPhams;
using BagStore.Web.Models.ViewModels.SanPhams;

namespace BagStore.Web.Services.Interfaces
{
    public interface IAnhSanPhamService
    {
        Task<BaseResponse<AnhSanPhamResponseDto>> CreateAsync(AnhSanPhamRequestDto dto);

        Task<BaseResponse<AnhSanPhamResponseDto>> UpdateAsync(int maAnh, AnhSanPhamRequestDto dto);

        Task<BaseResponse<bool>> DeleteAsync(int maAnh);

        Task<BaseResponse<AnhSanPhamResponseDto>> GetByIdAsync(int maAnh);

        Task<BaseResponse<List<AnhSanPhamResponseDto>>> GetBySanPhamIdAsync(int maSP);

        Task<BaseResponse<List<AnhSanPhamResponseDto>>> GetAllAsync();

        Task<BaseResponse<List<AnhSanPham>>> CreateMultipleAsync(int maSP, List<IFormFile> files);

        Task<BaseResponse<AnhSanPhamResponseDto>> SetPrimaryAsync(int maAnh);
    }
}