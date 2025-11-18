using HotelApi.src.HotelApi.Domain.DTOs;
using HotelApi.src.HotelApi.Domain.Entities;

namespace HotelApi.src.HotelApi.Core.Interfaces;

public interface IInvoiceService
{
    // Get all invoices with related booking, customer, and room data
    Task<IEnumerable<InvoiceDto>> GetAllInvoicesAsync();

    // Get a single invoice by its ID, including booking/customer/room
    Task<InvoiceDto?> GetInvoiceByIdAsync(int id);

    // Get invoice by booking ID
    Task<InvoiceDto?> GetInvoiceByBookingIdAsync(int bookingId);

    // Create a new invoice for a booking
    Task<InvoiceDto> CreateInvoiceAsync(int bookingId, decimal amount);

    // Register a payment for an invoice
    Task<bool> RegisterPaymentAsync(int invoiceId, decimal amountPaid, string paymentMethod);

    // Get unpaid invoices older than a certain number of days
    Task<IEnumerable<InvoiceDto>> GetUnpaidInvoicesOlderThanAsync(int days);
}