using System.ComponentModel.DataAnnotations.Schema;

namespace HotelApi.Data.Entities;

public class Invoice
{
    public int Id { get; set; }
    public int BookingId { get; set; }
    public Booking Booking { get; set; } = null!;
    [Column(TypeName = "decimal(10,2)")] public decimal AmountDue { get; set; }
    public DateTime IssueDate { get; set; } = DateTime.UtcNow;
    public InvoiceStatus Status { get; set; } = InvoiceStatus.Unpaid;
    public ICollection<Booking> Bookings = new List<Booking>();
    
}