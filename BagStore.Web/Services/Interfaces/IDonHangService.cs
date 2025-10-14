using BagStore.Web.Models.DTOs.Request;
using BagStore.Web.Models.DTOs.Response;

namespace BagStore.Web.Services.Interfaces
{
    public interface IDonHangService
    {
        Task<IEnumerable<DonHangPhanHoiDTO>> LayDonHangTheoKhachHangAsync(int maKhachHang);
        Task<DonHangPhanHoiDTO> TaoDonHangAsync(DonHangTaoDTO dto);
        Task<DonHangPhanHoiDTO> CapNhatTrangThaiAsync(DonHangCapNhatTrangThaiDTO dto);
    }
}
