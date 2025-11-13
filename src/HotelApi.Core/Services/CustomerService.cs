using System.Reflection.Metadata.Ecma335;
using HotelApi.src.HotelApi.Core.Interfaces;
using HotelApi.src.HotelApi.Data.Interfaces;
using HotelApi.src.HotelApi.Domain.Entities;

namespace HotelApi.src.HotelApi.Core.Services;

public class CustomerService(
    IGenericRepository<Customer> customerRepository,
    IGenericRepository<Booking> bookingRepository
    ) : ICustomerService
{
    private readonly IGenericRepository<Customer> _customerRepository = customerRepository;
    private readonly IGenericRepository<Booking> _bookingRepository = bookingRepository;

    public async Task<IEnumerable<Customer>> GetAllCustomersAsync() => await _customerRepository.GetAllAsync();
    public async Task<Customer?> GetCustomerByIdAsync(int id) => await _customerRepository.GetByIdAsync(id);
    public async Task<Customer> CreateCustomerAsync(Customer customer)
    {
        await _customerRepository.AddAsync(customer);
        await _customerRepository.SaveAsync();
        return customer;
    }
    public async Task<Customer?> UpdateCustomerAsync(int id, Customer updatedCustomer)
    {
        var existing = await _customerRepository.GetByIdAsync(id);
        if (existing == null) return null;
        existing.Firstname = updatedCustomer.Firstname;
        existing.Lastname = updatedCustomer.Lastname;
        existing.Email = updatedCustomer.Email;
        existing.Phone = updatedCustomer.Phone;

        _customerRepository.Update(existing);
        await _customerRepository.SaveAsync();
        return existing;
    }

    public async Task<bool> DeleteCustomerAsync(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null) return false;
        var bookings = await _bookingRepository.GetAllAsync();
        bool hasBookings = bookings.Any(b => b.CustomerId == id);
        if (hasBookings) return false;

        _customerRepository.Delete(customer);
        await _customerRepository.SaveAsync();
        return true;
    }
}