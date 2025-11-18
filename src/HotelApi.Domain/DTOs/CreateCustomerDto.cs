using System.ComponentModel.DataAnnotations;

namespace HotelApi.src.HotelApi.Domain.DTOs;

public class CreateCustomerDto
{
    [Required] public string FirstName { get; set; } = null!;
    [Required] public string LastName { get; set; } = null!;
    [EmailAddress] public string? Email { get; set; }
    public string? Phone { get; set; }

}