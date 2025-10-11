using BagStore.Web.Models.DTOs;

namespace BagStore.Web.Repositories.Interfaces
{
    public interface IChatLieuRepository
    {
        Task<List<ChatLieuDto>> GetAllAsync();

        Task<ChatLieuDto> GetByIdAsync(int maChatLieu);

        Task<int> CreateAsync(ChatLieuDto request);

        Task<bool> UpdateAsync(ChatLieuDto request);

        Task<bool> DeleteAsync(int maChatLieu);
    }
}