using HotelApi.src.HotelApi.Domain.Entities;
namespace HotelApi.src.HotelApi.Data.Interfaces;

public interface IPaymentRepository
{
    Task AddPaymentAsync(PaymentRecord payment);
    Task SaveAsync();
}