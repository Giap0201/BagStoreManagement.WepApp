using BagStore.Web.Models.DTOs;

namespace BagStore.Web.Services.Interfaces
{
    public interface IKichThuocService
    {
        Task<KichThuocDto> CreateAsync(KichThuocDto dto);

        Task<KichThuocDto> GetByIdAsync(int maKichThuoc);

        Task<List<KichThuocDto>> GetAllAsync();

        Task<KichThuocDto> UpdateAsync(int maKichThuoc, KichThuocDto dto);

        Task<bool> DeleteAsync(int maKichThuoc);
    }
}