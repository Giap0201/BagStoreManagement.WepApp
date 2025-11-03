using BagStore.Data;
using BagStore.Web.Models.Entities;
using BagStore.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BagStore.Web.Repositories.implementations
{
    public class KhachHangImpl : IKhachHangRepository
    {
        private readonly BagStoreDbContext _context;
        public KhachHangImpl(BagStoreDbContext context)
        {
            _context = context;
        }
        public async Task<KhachHang?> GetByApplicationUserIdAsync(string applicationUserId)
        {
            if (string.IsNullOrEmpty(applicationUserId))
                return null;

            return await _context.KhachHangs
                .Include(k => k.ApplicationUser) // nếu có navigation property
                .FirstOrDefaultAsync(k => k.ApplicationUserId == applicationUserId);
        }
    }
}
