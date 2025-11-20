using System.Reflection.Metadata.Ecma335;
using HotelApi.src.HotelApi.Core.Interfaces;
using HotelApi.src.HotelApi.Data.Interfaces;
using HotelApi.src.HotelApi.Domain.DTOs;
using HotelApi.src.HotelApi.Domain.Entities;

namespace HotelApi.src.HotelApi.Core.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<IEnumerable<CustomerDto>> GetAllAsync()
    {
        var customers = await _customerRepository.GetAllAsync();
        return customers.Select(c => new CustomerDto
        {
            CustomerId = c.CustomerId,
            Name = c.Name,
            Email = c.Email,
            Phone = c.Phone
        });
    }

    public async Task<CustomerDto?> GetByIdAsync(int id)
    {
        var c = await _customerRepository.GetByIdAsync(id);
        if (c == null) return null;

        return new CustomerDto
        {
            CustomerId = c.CustomerId,
            Name = c.Name,
            Email = c.Email,
            Phone = c.Phone
        };
    }

    public async Task<CustomerDto> CreateAsync(CreateCustomerDto dto)
    {
        var customer = new Customer
        {
            Name = dto.Name,
            Email = dto.Email,
            Phone = dto.Phone,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _customerRepository.AddAsync(customer);
        await _customerRepository.SaveAsync();

        return new CustomerDto
        {
            CustomerId = customer.CustomerId,
            Name = customer.Name,
            Email = customer.Email,
            Phone = customer.Phone
        };
    }

    public async Task<CustomerDto?> UpdateAsync(int id, UpdateCustomerDto dto)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null) return null;

        if (!string.IsNullOrWhiteSpace(dto.Name))
            customer.Name = dto.Name;

        if (!string.IsNullOrWhiteSpace(dto.Email))
            customer.Email = dto.Email;

        if (!string.IsNullOrWhiteSpace(dto.Phone))
            customer.Phone = dto.Phone;

        customer.UpdatedAt = DateTime.UtcNow;

        _customerRepository.Update(customer);
        await _customerRepository.SaveAsync();

        return new CustomerDto
        {
            CustomerId = customer.CustomerId,
            Name = customer.Name,
            Email = customer.Email,
            Phone = customer.Phone
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null) return false;

        _customerRepository.Delete(customer);
        await _customerRepository.SaveAsync();
        return true;
    }
    public async Task<List<CustomerDto>> SearchCustomersAsync(string search)
    {
        var customers = await _customerRepository.SearchCustomersAsync(search);

        return customers.Select(c => new CustomerDto
        {
            CustomerId = c.CustomerId,
            Name = c.Name,
            Email = c.Email,
            Phone = c.Phone
        }).ToList();
    }
}
