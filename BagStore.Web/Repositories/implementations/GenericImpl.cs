using BagStore.Data;
using BagStore.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BagStore.Web.Repositories.implementations
{
    public class GenericImpl<T> : IGenericRepository<T> where T : class
    {
        protected readonly BagStoreDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericImpl(BagStoreDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> LayTatCaAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> LayTheoIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task ThemAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task CapNhatAsync(T entity)
        {
            _dbSet.Update(entity);
        }

        public async Task XoaAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        public async Task LuuAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}