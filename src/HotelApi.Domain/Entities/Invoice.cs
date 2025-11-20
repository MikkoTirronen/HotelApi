using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HotelApi.src.HotelApi.Domain.Enums;

namespace HotelApi.src.HotelApi.Domain.Entities;

public class Invoice
{
    public int InvoiceId { get; set; }

    [Required]
    public int BookingId { get; set; }

    public Booking Booking { get; set; } = null!;

    [Column(TypeName = "decimal(10,2)")]
    public decimal AmountDue { get; set; }

    public DateTime IssueDate { get; set; }
    public DateTime? DueDate { get; set; }
    public InvoiceStatus Status { get; set; } = InvoiceStatus.Unpaid;
    public ICollection<PaymentRecord> Payments { get; set; } = new List<PaymentRecord>();
}