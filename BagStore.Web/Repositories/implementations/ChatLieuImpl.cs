using BagStore.Data;
using BagStore.Domain.Entities;
using BagStore.Web.Models.DTOs;
using BagStore.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BagStore.Web.Repositories.implementations
{
    public class ChatLieuImpl : IChatLieuRepository
    {
        private readonly BagStoreDbContext _context;

        public ChatLieuImpl(BagStoreDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAsync(ChatLieuDto request)
        {
            var entity = new ChatLieu
            {
                TenChatLieu = request.TenChatLieu,
                MoTa = request.MoTa
            };
            _context.ChatLieus.Add(entity);
            await _context.SaveChangesAsync();
            return entity.MaChatLieu;
        }

        public async Task<bool> DeleteAsync(int maChatLieu)
        {
            var entity = await _context.ChatLieus.FindAsync(maChatLieu);
            if (entity == null) return false;
            _context.ChatLieus.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ChatLieuDto>> GetAllAsync()
        {
            return await _context.ChatLieus.Select(x => new ChatLieuDto
            {
                MaChatLieu = x.MaChatLieu,
                TenChatLieu = x.TenChatLieu,
                MoTa = x.MoTa
            }).ToListAsync();
        }

        public async Task<ChatLieuDto> GetByIdAsync(int maChatLieu)
        {
            var entity = await _context.ChatLieus.FindAsync(maChatLieu);
            if (entity == null) return null;
            return new ChatLieuDto
            {
                MaChatLieu = entity.MaChatLieu,
                TenChatLieu = entity.TenChatLieu,
                MoTa = entity.MoTa
            };
        }

        public async Task<bool> UpdateAsync(ChatLieuDto request)
        {
            var entity = await _context.ChatLieus.FindAsync(request.MaChatLieu);
            if (entity == null) return false;
            entity.TenChatLieu = request.TenChatLieu;
            entity.MoTa = request.MoTa;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}