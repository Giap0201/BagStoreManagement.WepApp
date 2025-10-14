using BagStore.Data;
using BagStore.Domain.Entities;
using BagStore.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BagStore.Web.Repositories.implementations
{
    public class KichThuocImpl : IKichThuocRepository
    {
        private readonly BagStoreDbContext _context;

        public KichThuocImpl(BagStoreDbContext context)
        {
            _context = context;
        }

        public async Task<KichThuoc> AddAsync(KichThuoc entity)
        {
            _context.KichThuocs.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int maKichThuoc)
        {
            var entity = await _context.KichThuocs.FindAsync(maKichThuoc);
            if (entity == null) return false;
            _context.KichThuocs.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<KichThuoc>> GetAllAsync() => await _context.KichThuocs.ToListAsync();

        public async Task<KichThuoc> GetByIdAsync(int maKichThuoc) => await _context.KichThuocs.FindAsync(maKichThuoc);

        public async Task<KichThuoc> UpdateAsync(KichThuoc entity)
        {
            _context.KichThuocs.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}