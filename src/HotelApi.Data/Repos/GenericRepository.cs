using System.Threading.Tasks;
using HotelApi.src.HotelApi.Data.Contexts;
using HotelApi.src.HotelApi.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace HotelApi.src.HotelApi.Data.Repos;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly HotelDbContext _context;
    private readonly DbSet<T> _dbSet;
    public GenericRepository(HotelDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();
    public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);
    public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);
    public async Task SaveAsync() => await _context.SaveChangesAsync();
    public void Update(T entity) => _dbSet.Update(entity);
    public void Delete(T entity) => _dbSet.Remove(entity);
}