using BagStore.Domain.Entities;
using BagStore.Web.Models.DTOs;

namespace BagStore.Web.Repositories.Interfaces
{
    public interface IDanhMucLoaiTuiRepository
    {
        Task<List<DanhMucLoaiTui>> GetAllAsync();

        Task<DanhMucLoaiTui> GetByIdAsync(int maLoaiTui);

        Task<DanhMucLoaiTui> AddAsync(DanhMucLoaiTui entity);

        Task<DanhMucLoaiTui> UpdateAsync(DanhMucLoaiTui entity);

        Task<bool> DeleteAsync(int maLoaiTui);
    }
}