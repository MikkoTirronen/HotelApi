using HotelApi.src.HotelApi.Domain.Entities;

namespace HotelApi.src.HotelApi.Data.Interfaces;

public interface ICustomerRepository
{
    Task<List<Customer>> GetAllAsync();
    Task<Customer?> GetByIdAsync(int id);
    Task<Customer?> GetByEmailAsync(string email);
    Task AddAsync(Customer customer);
    void Update(Customer customer);
    void Delete(Customer customer);
    Task SaveAsync();
}