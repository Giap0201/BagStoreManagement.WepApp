using BagStore.Web.Models.DTOs;

namespace BagStore.Web.Repositories.Interfaces
{
    public interface IKichThuocRepository
    {
        Task<List<KichThuocDto>> GetAllAsync();

        Task<KichThuocDto> GetByIdAsync(int maKichThuoc);

        Task<int> CreateAsync(KichThuocDto request);

        Task<bool> UpdateAsync(KichThuocDto request);

        Task<bool> DeleteAsync(int maKichThuoc);
    }
}