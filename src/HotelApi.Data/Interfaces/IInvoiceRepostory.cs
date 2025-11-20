using HotelApi.src.HotelApi.Domain.Entities;
using HotelApi.src.HotelApi.Domain.Enums;

namespace HotelApi.src.HotelApi.Data.Interfaces;

public interface IInvoiceRepository : IGenericRepository<Invoice>
{
    // Task<IEnumerable<Invoice>> GetInvoicesWithBookingsAsync();
    // Task<Invoice?> GetInvoiceWithBookingsByIdAsync(int id);
    Task<IEnumerable<Invoice>> GetAllWithIncludesAsync();

    Task<Invoice?> GetInvoiceWithDetailsAsync(int id);
    Task<Invoice?> GetInvoiceWithCustomerAsync(int id);
    Task<List<Invoice>> GetUnpaidOlderThanAsync(DateTime thresholdDate);
    Task UpdateInvoicesAsync(List<Invoice> invoices);
    Task<Invoice?> GetInvoiceByBookingIdAsync(int bookingId);
    Task<List<Invoice>> SearchInvoicesAsync(
       int? customerId,
       InvoiceStatus? status,
       string? customerName);
}