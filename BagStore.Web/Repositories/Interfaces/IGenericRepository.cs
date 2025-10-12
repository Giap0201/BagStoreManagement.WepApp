namespace BagStore.Web.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> LayTatCaAsync();
        Task<T?> LayTheoIdAsync(int id);
        Task ThemAsync(T entity);
        Task CapNhatAsync(T entity);
        Task XoaAsync(int id);
        Task LuuAsync();
    }
}
