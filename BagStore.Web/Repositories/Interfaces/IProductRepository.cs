using BagStore.Domain.Entities;

namespace BagStore.Web.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<DanhMucLoaiTui>> GetAllAsync();

        Task<DanhMucLoaiTui?> GetByIdAsync(int id);

        Task AddAsync(DanhMucLoaiTui entity);
    }
}