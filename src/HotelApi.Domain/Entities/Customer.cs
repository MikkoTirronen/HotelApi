using System.ComponentModel.DataAnnotations;

namespace HotelApi.src.HotelApi.Domain.Entities;

public class Customer
{
    public int Id { get; set; }
    [Required] public string Firstname { get; set; } = null!;
    [Required] public string Lastname { get; set; } = null!;
    [EmailAddress] public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}