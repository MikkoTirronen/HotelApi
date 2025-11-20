namespace HotelApi.src.HotelApi.Domain.DTOs;

public class InvoiceListDto
{
    public int InvoiceId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty; // "paid" | "pending" | "overdue"
    public DateTime IssueDate { get; set; }
    public DateTime? DueDate { get; set; }
}