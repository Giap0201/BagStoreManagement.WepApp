using BagStore.Domain.Entities;

namespace BagStore.Web.Repositories.Interfaces
{
    public interface IDonHangRepository : IGenericRepository<DonHang>
    {
        Task<IEnumerable<DonHang>> LayDonHangTheoKhachHangAsync(int maKH);
        Task<IEnumerable<DonHang>> LayDonHangTheoUserAsync(string userId);
        Task<IEnumerable<DonHang>> LayDonHangTheoTrangThaiAsync(string trangThai);
        Task<IEnumerable<DonHang>> LayTatCaDonHangAsync();
        Task<DonHang?> LayTheoIdAsync(int maDonHang);
    }
}
