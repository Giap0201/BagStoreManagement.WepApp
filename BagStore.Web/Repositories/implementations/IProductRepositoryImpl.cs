using BagStore.Domain.Entities;
using BagStore.Web.Repositories.Interfaces;

namespace BagStore.Web.Repositories.implementations
{
    public class IProductRepositoryImpl : IProductRepository
    {
        public Task AddAsync(DanhMucLoaiTui entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DanhMucLoaiTui>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<DanhMucLoaiTui?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}