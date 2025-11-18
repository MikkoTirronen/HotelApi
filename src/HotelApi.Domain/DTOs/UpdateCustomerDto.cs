using System.ComponentModel.DataAnnotations;

namespace HotelApi.src.HotelApi.Domain.DTOs;

public class UpdateCustomerDto
{
    public string? Name { get; set; } = null!;

    [EmailAddress] public string? Email { get; set; }
    public string? Phone { get; set; }

}