using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HotelApi.src.HotelApi.Domain.Enums;


namespace HotelApi.src.HotelApi.Domain.Entities;

public class Room
{
    public int RoomId { get; set; }
    [Required] public string RoomNumber { get; set; } = null!;
    public RoomType Type { get; set; }
    public int BaseCapacity { get; set; }
    public int MaxExtraBeds { get; set; }
    [Column(TypeName = "decimal(10,2)")] public decimal PricePerNight { get; set; }
    public string? Amenities { get; set; }
    public bool Active { get; set; } = true;
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}