using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HotelApi.src.HotelApi.Domain.Enums;


namespace HotelApi.src.HotelApi.Domain.Entities;

public class Room
{
    public int Id { get; set; }
    [Required] public string Number { get; set; } = null!;
    public RoomType Type { get; set; }
    public int BaseCapacity { get; set; }
    public int MaxExtraBeds { get; set; }
    [Column(TypeName = "decimal(10,2)")] public decimal PricePerNight;
    public string? Amenities { get; set; }
    public bool Active { get; set; } = true;
    public ICollection<Booking> Bookings { get; set; } = [];
}