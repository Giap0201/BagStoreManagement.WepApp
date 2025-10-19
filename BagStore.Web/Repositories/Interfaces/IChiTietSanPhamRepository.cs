using BagStore.Domain.Entities;

namespace BagStore.Web.Repositories.Interfaces
{
    public interface IChiTietSanPhamRepository
    {
        Task<ChiTietSanPham?> GetByIdAsync(int maChiTietSanPham);

        //Lay toan bo bien the theo ma san pham
        Task<List<ChiTietSanPham>> GetBySanPhamIdAsync(int maSP);

        //lay ra cac bien the san pham khac nhau bao gom mau sac va kich thuoc
        Task<ChiTietSanPham?> FindByAttributesAsync(int maSP, int maMauSac, int maKichThuoc);

        Task<ChiTietSanPham> AddAsync(ChiTietSanPham entity);

        Task<ChiTietSanPham> UpdateAsync(ChiTietSanPham entity);

        Task<bool> DeleteAsync(int maChiTietSanPham);
    }
}