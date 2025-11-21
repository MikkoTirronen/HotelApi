using System.ComponentModel.DataAnnotations;

namespace HotelApi.src.HotelApi.Domain.DTOs;

public class CreateBookingWithCustomerDto
{
    public int RoomId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int NumPersons { get; set; }

    [Required] public string Name { get; set; } = null!;
    [EmailAddress] public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public int? ExtraBedsCount{ get; set; }
}