using BagStore.Domain.Entities;
using BagStore.Web.Models.DTOs;

namespace BagStore.Web.Repositories.Interfaces
{
    public interface IThuongHieuRepository
    {
        // Lấy tất cả thương hiệu
        Task<List<ThuongHieu>> GetAllAsync();

        // Lấy thương hiệu theo ID
        Task<ThuongHieu> GetByIdAsync(int maThuongHieu);

        // Thêm mới thương hiệu
        Task<ThuongHieu> AddAsync(ThuongHieu entity);

        // Cập nhật thương hiệu
        Task<ThuongHieu> UpdateAsync(ThuongHieu entity);

        // Xóa thương hiệu
        Task<bool> DeleteAsync(int maThuongHieu);

        Task<ThuongHieu> GetByNameAsync(string tenThuongHieu);
    }
}