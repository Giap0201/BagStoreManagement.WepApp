using BagStore.Domain.Entities;
using BagStore.Web.Models.DTOs;

namespace BagStore.Web.Repositories.Interfaces
{
    public interface IKichThuocRepository
    {
        Task<List<KichThuoc>> GetAllAsync();

        Task<KichThuoc> GetByIdAsync(int maKichThuoc);

        Task<KichThuoc> AddAsync(KichThuoc entity);

        Task<KichThuoc> UpdateAsync(KichThuoc entity);

        Task<bool> DeleteAsync(int maKichThuoc);

        Task<KichThuoc> GetByNameAsync(string tenKichThuoc);
    }
}