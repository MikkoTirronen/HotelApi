namespace HotelApi.src.HotelApi.Domain.DTOs;

public class UpdateBookingDto
{
    public int CustomerId { get; set; }
    public int RoomId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int? NumPersons { get; set; }
}