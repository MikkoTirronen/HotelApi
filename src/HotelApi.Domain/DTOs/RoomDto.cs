namespace HotelApi.src.HotelApi.Domain.DTOs;

public class RoomDto
{
    public int Id { get; set; }
    public string RoomNumber { get; set; } = null!;
    public decimal PricePerNight { get; set; }
    public int BaseCapacity { get; set; }
    public int MaxExtraBeds { get; set; }
    public string? Amenities { get; set; }
    public bool Active { get; set; }
   // public List<BookingDto> Bookings { get; set; } = new List<BookingDto>();
}