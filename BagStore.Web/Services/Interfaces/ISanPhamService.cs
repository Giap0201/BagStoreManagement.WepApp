using BagStore.Models.Common;
using BagStore.Web.Models.DTOs.SanPhams;
using BagStore.Web.Models.ViewModels.SanPhams;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BagStore.Web.Services.Interfaces
{
    public interface ISanPhamService
    {
        Task<BaseResponse<SanPhamResponseDto>> CreateAsync(SanPhamCreateDto dto);
        Task<BaseResponse<SanPhamResponseDto>> GetByIdAsync(int maSanPham);
        Task<BaseResponse<List<SanPhamResponseDto>>> GetAllAsync();
        Task<BaseResponse<SanPhamResponseDto>> UpdateAsync(int maSanPham, SanPhamUpdateDto dto);
        Task<BaseResponse<bool>> DeleteAsync(int maSanPham);

        // 🔥 Cập nhật lại hàm này để trùng với service
        Task<BaseResponse<PagedResult<SanPhamResponseDto>>> GetAllPagedAsync(
            int page, int pageSize, string? keyword,
            int? maLoaiTui = null, int? maThuongHieu = null, int? maChatLieu = null);
    }

}
