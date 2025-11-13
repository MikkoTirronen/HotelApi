using HotelApi.src.HotelApi.Domain.Entities;

namespace HotelApi.src.HotelApi.Core.Interfaces;

public interface IPaymentService
{
       Task<PaymentRecord> RegisterPaymentAsync(int invoiceId, decimal amount, string? method = null);
}