using BagStore.Web.Models.DTOs.Request;
using BagStore.Web.Models.DTOs.Response;

namespace BagStore.Web.Services.Interfaces
{
    public interface IDonHangService
    {
        Task<IEnumerable<DonHangResponse>> LayTatCaDonHangAsync();
        Task<IEnumerable<DonHangResponse>> LayDonHangTheoKhachHangAsync(int maKhachHang);
        Task<DonHangResponse> TaoDonHangAsync(CreateDonHangRequest dto);
        Task<DonHangResponse> CapNhatTrangThaiAsync(UpdateDonHangStatusRequest dto);
    }
}
