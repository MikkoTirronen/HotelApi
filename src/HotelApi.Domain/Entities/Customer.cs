using System.ComponentModel.DataAnnotations;

namespace HotelApi.src.HotelApi.Domain.Entities;

public class Customer
{
    public int CustomerId { get; set; }
    [Required] public string Name { get; set; } = null!;
    [EmailAddress] public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    //public string? Address { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ICollection<Booking>? Bookings { get; set; } = new List<Booking>();
}