using BagStore.Data;
using BagStore.Domain.Entities;
using BagStore.Web.Models.DTOs;
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

        public async Task<int> CreateAsync(MauSacDto request)
        {
            var entity = new MauSac
            {
                TenMauSac = request.TenMauSac
            };
            _context.MauSacs.Add(entity);
            await _context.SaveChangesAsync();
            return entity.MaMauSac;
        }

        public async Task<bool> DeleteAsync(int maMauSac)
        {
            var entity = await _context.MauSacs.FindAsync(maMauSac);
            if (entity == null) return false;
            _context.MauSacs.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<MauSacDto>> GetAllAsync()
        {
            return await _context.MauSacs.Select(x => new MauSacDto
            {
                MaMauSac = x.MaMauSac,
                TenMauSac = x.TenMauSac
            }).ToListAsync();
        }

        public async Task<MauSacDto> GetByIdAsync(int maMauSac)
        {
            var entity = await _context.MauSacs.FindAsync(maMauSac);
            if (entity == null) return null;
            return new MauSacDto
            {
                MaMauSac = entity.MaMauSac,
                TenMauSac = entity.TenMauSac
            };
        }

        public async Task<bool> UpdateAsync(MauSacDto request)
        {
            var entity = await _context.MauSacs.FindAsync(request.MaMauSac);
            if (entity == null) return false;
            entity.TenMauSac = request.TenMauSac;
            _context.MauSacs.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}