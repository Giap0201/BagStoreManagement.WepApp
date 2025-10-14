using BagStore.Web.Models.DTOs;

namespace BagStore.Web.Repositories.Interfaces
{
    public interface IMauSacRepository
    {
        Task<List<MauSacDto>> GetAllAsync();

        Task<MauSacDto> GetByIdAsync(int maMauSac);

        Task<int> CreateAsync(MauSacDto request);

        Task<bool> UpdateAsync(MauSacDto request);

        Task<bool> DeleteAsync(int maMauSac);
    }
}