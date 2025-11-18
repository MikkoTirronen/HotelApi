using HotelApi.src.HotelApi.Domain.Enums;

namespace HotelApi.src.HotelApi.Domain.DTOs;

public class BookingDto
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    public int CustomerId { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int NumPersons { get; set; }
    public decimal TotalPrice { get; set; }

    public BookingStatus Status { get; set; }

    public RoomDto? Room { get; set; }
    public CustomerDto? Customer { get; set; }
    public InvoiceDto? Invoice { get; set; }
}