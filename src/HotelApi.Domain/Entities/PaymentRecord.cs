using System.ComponentModel.DataAnnotations.Schema;

namespace HotelApi.src.HotelApi.Domain.Entities;

public class PaymentRecord
{
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public Invoice Invoice { get; set; } = null!;
    [Column(TypeName = "decimal(10,2)")] public decimal AmountDue { get; set; }

}