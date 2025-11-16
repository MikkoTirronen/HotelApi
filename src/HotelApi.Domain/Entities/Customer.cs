using System.ComponentModel.DataAnnotations;

namespace HotelApi.src.HotelApi.Domain.Entities;

public class Customer
{
    public int Id { get; set; }
    [Required] public string FirstName { get; set; } = null!;
    [Required] public string LastName { get; set; } = null!;
    [EmailAddress] public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public DateTime CreatedAt { get; set; } 
    public DateTime UpdatedAt { get; set; }
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}