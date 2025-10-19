using BagStore.Models.Common;
using BagStore.Web.Models.DTOs;

namespace BagStore.Web.Services.Interfaces
{
    public interface IThuongHieuService
    {
        Task<BaseResponse<ThuongHieuDto>> CreateAsync(ThuongHieuDto dto);

        Task<BaseResponse<ThuongHieuDto>> GetByIdAsync(int maThuongHieu);

        Task<BaseResponse<List<ThuongHieuDto>>> GetAllAsync();

        Task<BaseResponse<ThuongHieuDto>> UpdateAsync(int maThuongHieu, ThuongHieuDto dto);

        Task<BaseResponse<bool>> DeleteAsync(int maThuongHieu);
    }
}