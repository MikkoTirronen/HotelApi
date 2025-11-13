using HotelApi.src.HotelApi.Domain.Entities;

namespace HotelApi.src.HotelApi.Core.Interfaces;

public interface ICustomerService
{
    Task<IEnumerable<Customer>> GetAllCustomersAsync();
    Task<Customer?> GetCustomerByIdAsync();
    Task<Customer> CreateCustomerAsync(Customer customer);
    Task<Customer?> UpdateCustomerAsync(int id, Customer updatedCustomer);
    Task<bool> DeleteCustomerAsync(int id);
}