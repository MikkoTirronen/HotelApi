using System.ComponentModel.DataAnnotations.Schema;
using HotelApi.src.HotelApi.Domain.Enums;

namespace HotelApi.src.HotelApi.Domain.Entities;

public class Invoice
{
    public int Id { get; set; }
    public int BookingId { get; set; }
    public Booking Booking { get; set; } = null!;
    [Column(TypeName = "decimal(10,2)")] public decimal AmountDue { get; set; }
    public DateTime IssueDate { get; set; } = DateTime.UtcNow;
    public InvoiceStatus Status { get; set; } = InvoiceStatus.Unpaid;
    public ICollection<Booking> Bookings = [];

}