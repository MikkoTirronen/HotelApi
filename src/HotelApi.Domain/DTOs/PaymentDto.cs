namespace HotelApi.src.HotelApi.Domain.DTOs;

public record PaymentDto(
    int PaymentId,
    int InvoiceId,
    decimal AmountPaid,
    DateTime PaymentDate,
    string? PaymentMethod,
    string CustomerName
);