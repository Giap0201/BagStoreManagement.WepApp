using BagStore.Models.Common;
using BagStore.Web.Models.DTOs;

namespace BagStore.Web.Services.Interfaces
{
    public interface IChatLieuService
    {
        Task<BaseResponse<ChatLieuDto>> CreateAsync(ChatLieuDto dto);

        Task<BaseResponse<ChatLieuDto>> UpdateAsync(int maChatLieu, ChatLieuDto dto);

        Task<BaseResponse<bool>> DeleteAsync(int maChatLieu);

        Task<BaseResponse<ChatLieuDto>> GetByIdAsync(int maChatLieu);

        Task<BaseResponse<List<ChatLieuDto>>> GetAllAsync();
    }
}