using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HotelApi.src.HotelApi.Domain.Entities;

public class PaymentRecord
{
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public Invoice Invoice { get; set; } = null!;

    [Precision(10, 2)] public decimal AmountPaid { get; set; }

    public DateTime PaymentDate { get; set; }

    public string? PaymentMethod { get; set; }

}