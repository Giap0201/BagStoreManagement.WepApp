using BagStore.Models.Common;
using BagStore.Web.Models.DTOs;

namespace BagStore.Services.Interfaces
{
    public interface IDanhMucLoaiTuiService
    {
        Task<BaseResponse<DanhMucLoaiTuiDto>> CreateAsync(DanhMucLoaiTuiDto dto);

        Task<BaseResponse<bool>> DeleteAsync(int maLoaiTui);

        Task<BaseResponse<List<DanhMucLoaiTuiDto>>> GetAllAsync();

        Task<BaseResponse<DanhMucLoaiTuiDto>> GetByIdAsync(int maLoaiTui);

        Task<BaseResponse<DanhMucLoaiTuiDto>> UpdateAsync(int maLoaiTui, DanhMucLoaiTuiDto dto);
    }
}