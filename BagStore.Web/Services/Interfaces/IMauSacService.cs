using BagStore.Models.Common;
using BagStore.Web.Models.DTOs;

namespace BagStore.Web.Services.Interfaces
{
    public interface IMauSacService
    {
        Task<BaseResponse<MauSacDto>> CreateAsync(MauSacDto dto);

        Task<BaseResponse<MauSacDto>> UpdateAsync(int maMauSac, MauSacDto dto);

        Task<BaseResponse<bool>> DeleteAsync(int maMauSac);

        Task<BaseResponse<MauSacDto>> GetByIdAsync(int maMauSac);

        Task<BaseResponse<List<MauSacDto>>> GetAllAsync();
    }
}