using BagStore.Data;
using BagStore.Domain.Entities;
using BagStore.Web.Models.DTOs;
using BagStore.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BagStore.Web.Repositories.implementations
{
    public class DanhMucLoaiTuiImpl : IDanhMucLoaiTuiRepository
    {
        private readonly BagStoreDbContext _context;

        public DanhMucLoaiTuiImpl(BagStoreDbContext context)
        {
            _context = context;
        }

        public async Task<DanhMucLoaiTuiDto> CreateAsync(DanhMucLoaiTuiDto danhMucLoaiTuiDto)
        {
            var entity = new DanhMucLoaiTui
            {
                TenLoaiTui = danhMucLoaiTuiDto.TenLoaiTui,
                MoTa = danhMucLoaiTuiDto.MoTa
            };
            _context.DanhMucLoaiTuis.Add(entity);
            await _context.SaveChangesAsync();
            danhMucLoaiTuiDto.MaLoaiTui = entity.MaLoaiTui;
            return danhMucLoaiTuiDto;
        }

        public async Task<bool> DeleteAsync(int maLoaiTui)
        {
            var entity = await _context.DanhMucLoaiTuis.FindAsync(maLoaiTui);
            if (entity == null)
            {
                return false;
            }
            _context.DanhMucLoaiTuis.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<DanhMucLoaiTuiDto>> GetAllDanhMucLoaiTuiAsync()
        {
            return await _context.DanhMucLoaiTuis.Select(x => new DanhMucLoaiTuiDto
            {
                MaLoaiTui = x.MaLoaiTui,
                TenLoaiTui = x.TenLoaiTui,
                MoTa = x.MoTa
            }).ToListAsync();
        }

        public async Task<DanhMucLoaiTuiDto> GetDanhMucLoaiTuiByIdAsync(int maLoaiTui)
        {
            return await _context.DanhMucLoaiTuis.Where(x => x.MaLoaiTui == maLoaiTui)
                .Select(x => new DanhMucLoaiTuiDto
                {
                    MaLoaiTui = x.MaLoaiTui,
                    TenLoaiTui = x.TenLoaiTui,
                    MoTa = x.MoTa
                }).FirstOrDefaultAsync();
        }

        public async Task<DanhMucLoaiTuiDto> UpdateAsync(DanhMucLoaiTuiDto danhMucLoaiTuiDto)
        {
            var entity = await _context.DanhMucLoaiTuis.FindAsync(danhMucLoaiTuiDto.MaLoaiTui);
            if (entity == null)
            {
                return null;
            }
            entity.TenLoaiTui = danhMucLoaiTuiDto.TenLoaiTui;
            entity.MoTa = danhMucLoaiTuiDto.MoTa;
            _context.DanhMucLoaiTuis.Update(entity);
            await _context.SaveChangesAsync();
            return danhMucLoaiTuiDto;
        }
    }
}