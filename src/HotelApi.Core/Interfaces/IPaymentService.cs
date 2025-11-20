using HotelApi.src.HotelApi.Domain.DTOs;
using HotelApi.src.HotelApi.Domain.Entities;

namespace HotelApi.src.HotelApi.Core.Interfaces;

public interface IPaymentService
{
       Task<PaymentDto> RegisterPaymentAsync(int invoiceId, string customer, decimal amount, string? method = null);
       Task<IEnumerable<PaymentDto>> GetAllPaymentsAsync();
}