using System.ComponentModel.DataAnnotations;

namespace HotelApi.src.HotelApi.Domain.DTOs;

public class CustomerDto
{
    public int CustomerId { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    //public string? Address { get; set; }
}