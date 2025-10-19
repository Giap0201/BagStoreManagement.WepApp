using BagStore.Models.Common;
using BagStore.Web.Models.DTOs;

namespace BagStore.Web.Services.Interfaces
{
    public interface IKichThuocService
    {
        Task<BaseResponse<KichThuocDto>> CreateAsync(KichThuocDto dto);

        Task<BaseResponse<KichThuocDto>> GetByIdAsync(int maKichThuoc);

        Task<BaseResponse<List<KichThuocDto>>> GetAllAsync();

        Task<BaseResponse<KichThuocDto>> UpdateAsync(int maKichThuoc, KichThuocDto dto);

        Task<BaseResponse<bool>> DeleteAsync(int maKichThuoc);
    }
}