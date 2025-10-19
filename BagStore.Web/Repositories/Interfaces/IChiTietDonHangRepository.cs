using BagStore.Domain.Entities;

namespace BagStore.Web.Repositories.Interfaces
{
    public interface IChiTietDonHangRepository : IGenericRepository<ChiTietDonHang>
    {
        Task<IEnumerable<ChiTietDonHang>> LayTheoDonHangAsync(int maDonHang);
    }
}
