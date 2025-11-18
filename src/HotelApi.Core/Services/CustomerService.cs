using System.Reflection.Metadata.Ecma335;
using HotelApi.src.HotelApi.Core.Interfaces;
using HotelApi.src.HotelApi.Data.Interfaces;
using HotelApi.src.HotelApi.Domain.DTOs;
using HotelApi.src.HotelApi.Domain.Entities;

namespace HotelApi.src.HotelApi.Core.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repo;

    public CustomerService(ICustomerRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<CustomerDto>> GetAllAsync()
    {
        var customers = await _repo.GetAllAsync();
        return customers.Select(c => new CustomerDto
        {
            Id = c.Id,
            Name = c.Name,
            Email = c.Email,
            Phone = c.Phone
        });
    }

    public async Task<CustomerDto?> GetByIdAsync(int id)
    {
        var c = await _repo.GetByIdAsync(id);
        if (c == null) return null;

        return new CustomerDto
        {
            Id = c.Id,
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

        await _repo.AddAsync(customer);
        await _repo.SaveAsync();

        return new CustomerDto
        {
            Id = customer.Id,
            Name = customer.Name,     
            Email = customer.Email,
            Phone = customer.Phone
        };
    }

    public async Task<CustomerDto?> UpdateAsync(int id, UpdateCustomerDto dto)
    {
        var customer = await _repo.GetByIdAsync(id);
        if (customer == null) return null;

        if (!string.IsNullOrWhiteSpace(dto.Name))
            customer.Name = dto.Name;

        if (!string.IsNullOrWhiteSpace(dto.Email))
            customer.Email = dto.Email;

        if (!string.IsNullOrWhiteSpace(dto.Phone))
            customer.Phone = dto.Phone;

        customer.UpdatedAt = DateTime.UtcNow;

        _repo.Update(customer);
        await _repo.SaveAsync();

        return new CustomerDto
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            Phone = customer.Phone
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var customer = await _repo.GetByIdAsync(id);
        if (customer == null) return false;

        _repo.Delete(customer);
        await _repo.SaveAsync();
        return true;
    }
}
