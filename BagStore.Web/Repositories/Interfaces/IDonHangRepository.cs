using BagStore.Domain.Entities;

namespace BagStore.Web.Repositories.Interfaces
{
    public interface IDonHangRepository : IGenericRepository<DonHang>
    {
        Task<IEnumerable<DonHang>> LayDonHangTheoKhachHangAsync(int maKH);
        Task<IEnumerable<DonHang>> LayDonHangTheoTrangThaiAsync(string trangThai);
    }
}
