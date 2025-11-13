using HotelApi.src.HotelApi.Core.Interfaces;
using HotelApi.src.HotelApi.Data.Interfaces;
using HotelApi.src.HotelApi.Domain.Entities;
using HotelApi.src.HotelApi.Domain.Enums;

namespace HotelApi.src.HotelApi.Core.Services;

public class InvoiceService(IGenericRepository<Invoice> invoiceRepository, IGenericRepository<Booking> bookingRepository) : IInvoiceService
{
    private readonly IGenericRepository<Invoice> _invoiceRepository = invoiceRepository;
    private readonly IGenericRepository<Booking> _bookingRepository = bookingRepository;

    public async Task<IEnumerable<Invoice>> GetAllInvoicesAsync() => await _invoiceRepository.GetAllAsync();
    public async Task<Invoice?> GetInvoiceByIdAsync(int id) => await _invoiceRepository.GetByIdAsync(id);

    public async Task<Invoice?> GetInvoiceByBookingIdAsync(int bookingId)
    {
        var invoices = await _invoiceRepository.GetAllAsync();
        return invoices.FirstOrDefault(i => i.BookingId == bookingId);
    }

    public async Task<Invoice> CreateInvoiceAsync(int bookingId, decimal amount)
    {
        var booking = await _bookingRepository.GetByIdAsync(bookingId) ?? throw new Exception("Booking not found.");

        var invoice = new Invoice
        {
            BookingId = bookingId,
            AmountDue = amount,
            IssueDate = DateTime.UtcNow,
            Status = InvoiceStatus.Unpaid,
            Payments= []
        };

        await _invoiceRepository.AddAsync(invoice);
        await _invoiceRepository.SaveAsync();
        return invoice;
    }

    public async Task<bool> RegisterPaymentAsync(int invoiceId)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(invoiceId);
        if (invoice == null) return false;

        invoice.Status = InvoiceStatus.Paid;
        invoice.PaymentDate = DateTime.UtcNow;

        _invoiceRepository.Update(invoice);
        await _invoiceRepository.SaveAsync();
        return true;
    }
    public async Task<IEnumerable<Invoice>> GetUnpaidInvoicesOlderThanAsync(int days)
    {
        var invoices = await _invoiceRepository.GetAllAsync();
        var cutoff = DateTime.UtcNow.AddDays(-days);

        return [.. invoices.Where(i => i.Status == InvoiceStatus.Unpaid && i.IssueDate < cutoff)];
    }

}