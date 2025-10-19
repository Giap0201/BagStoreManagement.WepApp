using BagStore.Data;
using BagStore.Domain.Entities;
using BagStore.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BagStore.Web.Repositories.Implementations
{
    public class ChatLieuImpl : IChatLieuRepository
    {
        private readonly BagStoreDbContext _context;

        public ChatLieuImpl(BagStoreDbContext context)
        {
            _context = context;
        }

        // Thêm mới chất liệu
        public async Task<ChatLieu> AddAsync(ChatLieu entity)
        {
            _context.ChatLieus.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        // Xóa chất liệu
        public async Task<bool> DeleteAsync(int maChatLieu)
        {
            var entity = await _context.ChatLieus.FindAsync(maChatLieu);
            if (entity == null) return false;

            _context.ChatLieus.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        // Lấy tất cả chất liệu
        public async Task<List<ChatLieu>> GetAllAsync()
        {
            return await _context.ChatLieus
                        .AsNoTracking()
                        .ToListAsync();
        }

        // Lấy chất liệu theo ID
        public async Task<ChatLieu> GetByIdAsync(int maChatLieu)
        {
            return await _context.ChatLieus.FindAsync(maChatLieu);
        }

        // Lấy chất liệu theo tên
        public async Task<ChatLieu> GetByNameAsync(string tenChatLieu)
        {
            return await _context.ChatLieus
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.TenChatLieu == tenChatLieu);
        }

        // Cập nhật chất liệu
        public async Task<ChatLieu> UpdateAsync(ChatLieu entity)
        {
            _context.ChatLieus.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}