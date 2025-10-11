using BagStore.Data;
using BagStore.Domain.Entities;
using BagStore.Web.Models.DTOs;
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

        public async Task<int> CreateAsync(KichThuocDto request)
        {
            var entity = new KichThuoc
            {
                TenKichThuoc = request.TenKichThuoc,
                ChieuDai = request.ChieuDai,
                ChieuRong = request.ChieuRong,
                ChieuCao = request.ChieuCao
            };
            _context.KichThuocs.Add(entity);
            await _context.SaveChangesAsync();
            return entity.MaKichThuoc;
        }

        public async Task<bool> DeleteAsync(int maKichThuoc)
        {
            var entity = await _context.KichThuocs.FindAsync(maKichThuoc);
            if (entity == null) return false;
            _context.KichThuocs.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<KichThuocDto>> GetAllAsync()
        {
            return await _context.KichThuocs.Select(x => new KichThuocDto
            {
                MaKichThuoc = x.MaKichThuoc,
                TenKichThuoc = x.TenKichThuoc,
                ChieuDai = x.ChieuDai,
                ChieuRong = x.ChieuRong,
                ChieuCao = x.ChieuCao
            }).ToListAsync();
        }

        public async Task<KichThuocDto> GetByIdAsync(int maKichThuoc)
        {
            var entity = await _context.KichThuocs.FindAsync(maKichThuoc);
            if (entity == null) return null;
            return new KichThuocDto
            {
                MaKichThuoc = entity.MaKichThuoc,
                TenKichThuoc = entity.TenKichThuoc,
                ChieuDai = entity.ChieuDai,
                ChieuRong = entity.ChieuRong,
                ChieuCao = entity.ChieuCao
            };
        }

        public async Task<bool> UpdateAsync(KichThuocDto request)
        {
            var entity = await _context.KichThuocs.FindAsync(request.MaKichThuoc);
            if (entity == null) return false;
            entity.TenKichThuoc = request.TenKichThuoc;
            entity.ChieuDai = request.ChieuDai;
            entity.ChieuRong = request.ChieuRong;
            entity.ChieuCao = request.ChieuCao;
            _context.KichThuocs.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}