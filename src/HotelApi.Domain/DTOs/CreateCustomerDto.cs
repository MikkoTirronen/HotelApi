using System.ComponentModel.DataAnnotations;

namespace HotelApi.src.HotelApi.Domain.DTOs;

public class CreateCustomerDto
{
    [Required] public string Name { get; set; } = null!;
    [EmailAddress] public string Email { get; set; } = null!;
    public string? Phone { get; set; }

}