using HotelApi.src.HotelApi.Domain.Entities;

namespace HotelApi.src.HotelApi.Core.Interfaces;

public interface IInvoiceService
{
    Task<IEnumerable<Invoice>> GetAllInvoicesAsync();
    Task<Invoice?> GetInvoiceByIdAsync(int id);
    Task<Invoice?> GetInvoiceByBookingIdAsync(int bookingId);
    Task<Invoice> CreateInvoiceAsync(int bookingId, decimal amount);
    Task<bool> RegisterPaymentAsync(int invoiceId);
    Task<IEnumerable<Invoice>> GetUnpaidInvoicesOlderThanAsync(int days);
}