using BagStore.Domain.Entities;

namespace BagStore.Web.Repositories.Interfaces
{
    public interface IAnhSanPhamRepository
    {
        /// Thêm ảnh sản phẩm mới vào DB.
        Task<AnhSanPham> AddAsync(AnhSanPham anhSanPham);

        /// Xóa ảnh theo mã ảnh.
        Task<bool> DeleteAsync(int maAnh);

        /// Lấy danh sách ảnh theo mã sản phẩm.
        Task<List<AnhSanPham>> GetBySanPhamIdAsync(int maSP);

        /// Lấy thông tin 1 ảnh theo ID (tuỳ chọn).
        Task<AnhSanPham?> GetByIdAsync(int maAnh);

        /// Đặt 1 ảnh làm hình chính (bỏ hình chính cũ nếu có).
        Task<bool> SetMainImageAsync(int maSP, int maAnh);

        Task<AnhSanPham> UpdateAsync(AnhSanPham entity);
    }
}