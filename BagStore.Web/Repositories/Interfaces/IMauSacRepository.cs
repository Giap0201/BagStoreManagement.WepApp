using BagStore.Domain.Entities;
using BagStore.Web.Models.DTOs;

namespace BagStore.Web.Repositories.Interfaces
{
    public interface IMauSacRepository
    {
        Task<List<MauSac>> GetAllAsync();

        Task<MauSac> GetByIdAsync(int maMauSac);

        Task<MauSac> AddAsync(MauSac entity);

        Task<MauSac> UpdateAsync(MauSac entity);

        Task<bool> DeleteAsync(int maMauSac);

        Task<MauSac> GetByNameAsync(string tenMauSac);
    }
}