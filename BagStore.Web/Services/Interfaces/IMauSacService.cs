using BagStore.Web.Models.DTOs;

namespace BagStore.Web.Services.Interfaces
{
    public interface IMauSacService
    {
        Task<List<MauSacDto>> GetAllAsync();

        Task<MauSacDto> GetByIdAsync(int maMauSac);

        Task<MauSacDto> CreateAsync(MauSacDto dto);

        Task<MauSacDto> UpdateAsync(int maMauSac, MauSacDto dto);

        Task<bool> DeleteAsync(int maMauSac);
    }
}