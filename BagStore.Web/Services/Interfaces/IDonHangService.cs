using BagStore.Web.Models.DTOs.Requests;
using BagStore.Web.Models.DTOs.Response;

namespace BagStore.Web.Services.Interfaces
{
    public interface IDonHangService
    {
        Task<IEnumerable<DonHangResponse>> LayTatCaDonHangAsync();
        Task<IEnumerable<DonHangResponse>> LayDonHangTheoKhachHangAsync(int maKhachHang);
        Task<IEnumerable<DonHangResponse>> LayDonHangTheoUserAsync(string userId);
        Task<DonHangResponse> TaoDonHangAsync(CreateDonHangRequest request, string userId);
        Task<DonHangResponse> CapNhatTrangThaiAsync(UpdateDonHangStatusRequest dto);
    }
}
