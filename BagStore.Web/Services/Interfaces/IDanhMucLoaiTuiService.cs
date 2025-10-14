using BagStore.Web.Models.DTOs;

namespace BagStore.Web.Services.Interfaces
{
    public interface IDanhMucLoaiTuiService
    {
        Task<DanhMucLoaiTuiDto> CreateAsync(DanhMucLoaiTuiDto dto);

        Task<DanhMucLoaiTuiDto> GetByIdAsync(int maLoaiTui);

        Task<List<DanhMucLoaiTuiDto>> GetAllAsync();

        Task<DanhMucLoaiTuiDto> UpdateAsync(int maLoaiTui, DanhMucLoaiTuiDto dto);

        Task<bool> DeleteAsync(int maLoaiTui);
    }
}