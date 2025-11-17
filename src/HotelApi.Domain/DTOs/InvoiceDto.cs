using HotelApi.src.HotelApi.Domain.Enums;

namespace HotelApi.src.HotelApi.Domain.DTOs;

public class InvoiceDto
{
    public int Id { get; set; }
    public decimal AmountDue { get; set; }
    public DateTime IssueDate { get; set; }
    public InvoiceStatus Status { get; set; }
}