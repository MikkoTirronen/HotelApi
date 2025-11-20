using HotelApi.src.HotelApi.Domain.DTOs;
using HotelApi.src.HotelApi.Domain.Entities;
namespace HotelApi.src.HotelApi.Data.Interfaces;

public interface IPaymentRepository
{
    Task AddPaymentAsync(PaymentRecord payment);

    Task<IEnumerable<PaymentRecord>> GetAllPaymentsOrderedAsync();
    Task SaveAsync();
}