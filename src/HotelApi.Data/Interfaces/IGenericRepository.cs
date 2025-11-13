using HotelApi.src.HotelApi.Data.Contexts;
namespace HotelApi.src.HotelApi.Data.Interfaces;
public interface IGenericRepository<T> where T : class
{
    public Task<IEnumerable<T>> GetAllAsync();
    public Task<T?> GetByIdAsync(int id);
    public Task AddAsync(T entity);
    public Task SaveAsync();
    public void Update(T entity);
    public void Delete(T entity);
}