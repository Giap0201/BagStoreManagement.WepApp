using BagStore.Web.Models.DTOs;

namespace BagStore.Web.Services.Interfaces
{
    public interface IChatLieuService
    {
        Task<ChatLieuDto> CreateAsync(ChatLieuDto dto);

        Task<ChatLieuDto> GetByIdAsync(int maChatLieu);

        Task<List<ChatLieuDto>> GetAllAsync();

        Task<ChatLieuDto> UpdateAsync(int maChatLieu, ChatLieuDto dto);

        Task<bool> DeleteAsync(int maChatLieu);
    }
}