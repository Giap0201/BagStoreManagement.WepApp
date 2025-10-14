using BagStore.Data;
using BagStore.Domain.Entities;
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

        public async Task<ChatLieu> AddAsync(ChatLieu entity)
        {
            _context.ChatLieus.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int maChatLieu)
        {
            var entity = await _context.ChatLieus.FindAsync(maChatLieu);
            if (entity == null) return false;

            _context.ChatLieus.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ChatLieu>> GetAllAsync()
        {
            return await _context.ChatLieus.ToListAsync();
        }

        public async Task<ChatLieu> GetByIdAsync(int maChatLieu)
        {
            return await _context.ChatLieus.FindAsync(maChatLieu);
        }

        public async Task<ChatLieu> UpdateAsync(ChatLieu entity)
        {
            _context.ChatLieus.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}