using BagStore.Domain.Entities;

namespace BagStore.Web.Repositories.Interfaces
{
    public interface IChiTietDonHangRepository
    {
        Task AddAsync(ChiTietDonHang entity);
        Task<IEnumerable<ChiTietDonHang>> LayTheoDonHangAsync(int maDonHang);
        Task XoaAsync(ChiTietDonHang entity);
    }
}
