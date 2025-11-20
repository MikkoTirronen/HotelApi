using System.Reflection.Metadata.Ecma335;
using HotelApi.src.HotelApi.Data.Contexts;
using HotelApi.src.HotelApi.Data.Interfaces;
using HotelApi.src.HotelApi.Domain.DTOs;
using HotelApi.src.HotelApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelApi.src.HotelApi.Data.Repos;

public class CustomerRepository : ICustomerRepository
{
    private readonly HotelDbContext _context;
    public CustomerRepository(HotelDbContext context) => _context = context;

    public async Task<List<Customer>> GetAllAsync() => await _context.Customers.ToListAsync();

    public async Task<Customer?> GetByIdAsync(int id)
    {
        return await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == id);
    }
    public async Task<Customer?> GetByEmailAsync(string email)
    {
        return await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);
    }
    public async Task AddAsync(Customer customer) => await _context.Customers.AddAsync(customer);
    public void Delete(Customer customer) => _context.Customers.Remove(customer);
    public void Update(Customer customer) => _context.Customers.Update(customer);
    public async Task SaveAsync() => await _context.SaveChangesAsync();

    public async Task<List<Customer>> SearchCustomersAsync(string search)
    {
        if (string.IsNullOrWhiteSpace(search))
            return new List<Customer>();

        search = search.Trim().ToLower();

        var results = await _context.Customers
            .AsNoTracking()
            .Where(c =>
                EF.Functions.ILike(c.Name, $"%{search}%") ||
                EF.Functions.ILike(c.Email, $"%{search}%") ||
                c.Phone != null && EF.Functions.ILike(c.Phone, $"%{search}%")
            )
            .OrderBy(c => c.Name)
            .Take(10)

            .ToListAsync();
        return results;
    }

}