using BagStore.Data;
using BagStore.Domain.Entities;
using BagStore.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BagStore.Web.Repositories.implementations
{
    public class MauSacImpl : IMauSacRepository
    {
        private readonly BagStoreDbContext _context;

        public MauSacImpl(BagStoreDbContext context)
        {
            _context = context;
        }

        public async Task<MauSac> AddAsync(MauSac entity)
        {
            _context.MauSacs.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int maMauSac)
        {
            var entity = await _context.MauSacs.FindAsync(maMauSac);
            if (entity == null) return false;
            _context.MauSacs.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<MauSac>> GetAllAsync() => await _context.MauSacs.ToListAsync();

        public async Task<MauSac> GetByIdAsync(int maMauSac) => await _context.MauSacs.FindAsync(maMauSac);

        public async Task<MauSac> UpdateAsync(MauSac entity)
        {
            _context.MauSacs.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}