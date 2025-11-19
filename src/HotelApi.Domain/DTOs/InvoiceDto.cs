using HotelApi.src.HotelApi.Domain.Enums;

namespace HotelApi.src.HotelApi.Domain.DTOs;

public class InvoiceDto
{
    public int InvoiceId { get; set; }
    public int BookingId { get; set; }
    public decimal AmountDue { get; set; }
    public DateTime IssueDate { get; set; }
    public InvoiceStatus Status { get; set; }
    //public BookingDto? Booking { get; set; }
}