using BagStore.Web.Models.DTOs;

namespace BagStore.Web.Services.Interfaces
{
    public interface IThuongHieuService
    {
        Task<ThuongHieuDto> CreateAsync(ThuongHieuDto dto);

        Task<ThuongHieuDto> GetByIdAsync(int maThuongHieu);

        Task<List<ThuongHieuDto>> GetAllAsync();

        Task<ThuongHieuDto> UpdateAsync(int maThuongHieu, ThuongHieuDto dto);

        Task<bool> DeleteAsync(int maThuongHieu);
    }
}