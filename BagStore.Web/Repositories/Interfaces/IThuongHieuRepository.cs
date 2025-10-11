using BagStore.Domain.Entities;
using BagStore.Web.Models.DTOs;

namespace BagStore.Web.Repositories.Interfaces
{
    public interface IThuongHieuRepository
    {
        Task<List<ThuongHieuDto>> GetAllAsync();

        Task<ThuongHieuDto> GetByIdAsync(int maThuongHieu);

        Task<int> CreateAsync(ThuongHieuDto request);

        Task<bool> UpdateAsync(ThuongHieuDto request);

        Task<bool> DeleteAsync(int maThuongHieu);
    }
}