using BagStore.Domain.Entities;
using BagStore.Web.Models.DTOs;

namespace BagStore.Web.Repositories.Interfaces
{
    public interface IDanhMucLoaiTuiRepository
    {
        Task<List<DanhMucLoaiTuiDto>> GetAllDanhMucLoaiTuiAsync();

        Task<DanhMucLoaiTuiDto> GetDanhMucLoaiTuiByIdAsync(int maLoaiTui);

        Task<DanhMucLoaiTuiDto> CreateAsync(DanhMucLoaiTuiDto danhMucLoaiTuiDto);

        Task<DanhMucLoaiTuiDto> UpdateAsync(DanhMucLoaiTuiDto danhMucLoaiTuiDto);

        Task<bool> DeleteAsync(int maLoaiTui);
    }
}