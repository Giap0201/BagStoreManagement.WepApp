using BagStore.Domain.Entities;

namespace BagStore.Web.Repositories.Interfaces
{
    public interface IDonHangRepository
    {
        Task<IEnumerable<DonHang>> LayTatCaDonHangAsync();
        Task<IEnumerable<DonHang>> LayDonHangTheoUserAsync(string userId);
        Task<IEnumerable<DonHang>> LayDonHangTheoKhachHangAsync(int maKhachHang);
        Task AddAsync(DonHang entity);
        Task UpdateAsync(DonHang entity);
        Task SaveAsync(); // hoặc LuuAsync nếu bạn dùng tên khác
        Task<DonHang?> LayTheoIdAsync(int maDonHang); // dùng khi chỉ cần đơn hàng cơ bản
        Task<DonHang?> GetByIdWithDetailsAsync(int maDonHang); // include ChiTietDonHangs, ChiTietSanPham, SanPham, ...
        Task CapNhatAsync(DonHang entity);
        Task LuuAsync();
    }
}
