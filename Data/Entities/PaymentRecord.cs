using System.ComponentModel.DataAnnotations.Schema;

namespace HotelApi.Data.Entities;

public class PaymentRecord
{
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public Invoice Invoice { get; set; }
    [Column(TypeName = "decimal(10,2)")] public decimal AmountDue { get; set; }
    
}