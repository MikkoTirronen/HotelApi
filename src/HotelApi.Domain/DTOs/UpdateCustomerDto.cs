using System.ComponentModel.DataAnnotations;

namespace HotelApi.src.HotelApi.Domain.DTOs;

public class UpdateCustomerDto
{
    public string? FirstName { get; set; } = null!;
    public string? LastName { get; set; } = null!;
    [EmailAddress] public string? Email { get; set; }
    public string? Phone { get; set; }

}