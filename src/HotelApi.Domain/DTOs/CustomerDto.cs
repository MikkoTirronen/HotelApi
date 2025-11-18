using System.ComponentModel.DataAnnotations;

namespace HotelApi.src.HotelApi.Domain.DTOs;

public class CustomerDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Email { get; set; } 
    public string? Phone { get; set; }
    //public string? Address { get; set; }
}