using HotelApi.src.HotelApi.Domain.Enums;

namespace HotelApi.src.HotelApi.Domain.DTOs;

public class BookingDto
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int NumPersons { get; set; }
    public decimal TotalPrice { get; set; }
    public RoomDto Room { get; set; } = null!;
    public InvoiceStatus Status { get; set; } = InvoiceStatus.Unpaid;
    public CustomerDto Customer { get; set; } = null!;
    public InvoiceDto Invoice { get; set; } = null!;


}