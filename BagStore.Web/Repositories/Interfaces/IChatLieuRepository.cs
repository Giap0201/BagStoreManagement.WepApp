using BagStore.Domain.Entities;
using BagStore.Web.Models.DTOs;

namespace BagStore.Web.Repositories.Interfaces
{
    public interface IChatLieuRepository
    {
        Task<List<ChatLieu>> GetAllAsync();

        Task<ChatLieu> GetByIdAsync(int maChatLieu);

        Task<ChatLieu> AddAsync(ChatLieu entity);

        Task<ChatLieu> UpdateAsync(ChatLieu entity);

        Task<bool> DeleteAsync(int maChatLieu);

        Task<ChatLieu> GetByNameAsync(string tenChatlieu);
    }
}