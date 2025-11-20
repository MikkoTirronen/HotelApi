using HotelApi.src.HotelApi.Domain.DTOs;
using HotelApi.src.HotelApi.Domain.Entities;
using HotelApi.src.HotelApi.Domain.Enums;

namespace HotelApi.src.HotelApi.Core.Interfaces;

public interface IInvoiceService
{
    // Task<InvoiceListDto?> GetInvoiceListByIdAsync(int id);
    Task<IEnumerable<InvoiceListDto>> GetAllInvoiceListAsync();
    // Task<IEnumerable<InvoiceDto>> GetAllInvoicesAsync();
    Task<InvoiceDto?> GetInvoiceByIdAsync(int id);
    Task<InvoiceDto?> GetInvoiceByBookingIdAsync(int bookingId);
    Task<InvoiceDto> CreateInvoiceAsync(int bookingId, decimal amount);

    // Register a payment for an invoice
    // Task<bool> RegisterPaymentAsync(int invoiceId, decimal amountPaid, string paymentMethod);

    // Get unpaid invoices older than a certain number of days
    // Task<IEnumerable<InvoiceDto>> GetUnpaidInvoicesOlderThanAsync(int days);
    Task<List<InvoiceListDto>> SearchInvoicesAsync(
       int? customerId,
       InvoiceStatus? status,
       string? customerName);
    Task<InvoiceListDto?> UpdateInvoiceAsync(int id, InvoiceListDto dto);

    Task<int> VoidOldInvoicesAsync(int daysThreshold = 10);

}