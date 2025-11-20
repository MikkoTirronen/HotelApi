using HotelApi.src.HotelApi.Core.Interfaces;
using HotelApi.src.HotelApi.Data.Contexts;
using HotelApi.src.HotelApi.Data.Interfaces;
using HotelApi.src.HotelApi.Domain.DTOs;
using HotelApi.src.HotelApi.Domain.Entities;
using HotelApi.src.HotelApi.Domain.Enums;
using Microsoft.EntityFrameworkCore;

public class InvoiceService : IInvoiceService
{
    private readonly HotelDbContext _context;
    private readonly IInvoiceRepository _invoiceRepository;
    public InvoiceService(HotelDbContext context, IInvoiceRepository invoiceRepository)
    {
        _context = context;
        _invoiceRepository = invoiceRepository;
    }
    public async Task<IEnumerable<InvoiceListDto>> GetAllInvoiceListAsync()
    {
        var invoices = await _invoiceRepository.GetAllWithIncludesAsync();
        return invoices.Select(MapToInvoiceListDto);
    }

    // public async Task<InvoiceListDto?> GetInvoiceListByIdAsync(int id)
    // {
    //     var invoice = await _invoiceRepository.GetByIdWithIncludesAsync(id);
    //     return invoice == null ? null : MapToInvoiceListDto(invoice);
    // }

    // public async Task<IEnumerable<InvoiceDto>> GetAllInvoicesAsync()
    // {
    //     var invoices = await _context.Invoices
    //         .Include(i => i.Booking)
    //             .ThenInclude(b => b.Customer)
    //         .Include(i => i.Booking)
    //             .ThenInclude(b => b.Room)
    //         .ToListAsync();

    //     return invoices.Select(MapToDto).ToList();
    // }

    public async Task<InvoiceDto?> GetInvoiceByIdAsync(int id)
    {
        var invoice = await _invoiceRepository.GetInvoiceWithDetailsAsync(id);

        if (invoice == null) return null;

        return MapToDto(invoice);
    }

    public async Task<InvoiceDto?> GetInvoiceByBookingIdAsync(int bookingId)
    {
        var invoice = await _invoiceRepository.GetInvoiceByBookingIdAsync(bookingId);
        return invoice == null ? null : MapToDto(invoice);
    }

    public async Task<InvoiceDto> CreateInvoiceAsync(int bookingId, decimal amount)
    {
        var invoice = new Invoice
        {
            BookingId = bookingId,
            AmountDue = amount,
            IssueDate = DateTime.UtcNow,
            Status = InvoiceStatus.Unpaid
        };

        await _context.Invoices.AddAsync(invoice);
        await _context.SaveChangesAsync();

        return MapToDto(invoice);
    }

    public async Task<InvoiceListDto?> UpdateInvoiceAsync(int id, InvoiceListDto dto)
    {
        var invoice = await _invoiceRepository.GetInvoiceWithCustomerAsync(id);

        if (invoice == null)
            return null;

        // Update invoice values
        invoice.AmountDue = dto.Amount;
        invoice.IssueDate = dto.IssueDate;
        invoice.DueDate = dto.DueDate;

        invoice.Status = dto.Status.ToLower() switch
        {
            "paid" => InvoiceStatus.Paid,
            "unpaid" => InvoiceStatus.Unpaid,
            "void" => InvoiceStatus.Void,
            "partial" => InvoiceStatus.Partial,
            "unknown" => InvoiceStatus.Unknown,
            _ => invoice.Status
        };

        await _invoiceRepository.SaveAsync();

        // return updated DTO
        return new InvoiceListDto
        {
            InvoiceId = invoice.InvoiceId,
            CustomerName = invoice.Booking.Customer.Name,
            Amount = invoice.AmountDue,
            Status = invoice.Status.ToString().ToLower(),
            IssueDate = invoice.IssueDate,
            DueDate = invoice.DueDate
        };
    }
    // public async Task<bool> RegisterPaymentAsync(int invoiceId, decimal amountPaid, string paymentMethod)
    // {
    //     if (amountPaid <= 0)
    //         throw new ArgumentException("Payment amount must be positive.", nameof(amountPaid));

    //     // Load invoice with payments
    //     var invoice = await _context.Invoices
    //         .Include(i => i.Payments) // Include payment history
    //         .FirstOrDefaultAsync(i => i.InvoiceId == invoiceId);

    //     if (invoice == null) return false;

    //     // Calculate total paid so far
    //     var totalPaid = invoice.Payments.Sum(p => p.AmountPaid);

    //     // Prevent overpayment
    //     if (totalPaid + amountPaid > invoice.AmountDue)
    //         throw new InvalidOperationException("Payment exceeds invoice amount.");

    //     // Create new payment record
    //     var payment = new PaymentRecord
    //     {
    //         InvoiceId = invoice.InvoiceId,
    //         AmountPaid = amountPaid,
    //         PaymentDate = DateTime.UtcNow,
    //         PaymentMethod = paymentMethod
    //     };
    //     await _context.Payments.AddAsync(payment);

    //     // Update invoice status based on cumulative payments
    //     var newTotalPaid = totalPaid + amountPaid;
    //     if (newTotalPaid == invoice.AmountDue)
    //         invoice.Status = InvoiceStatus.Paid;
    //     else if (newTotalPaid > 0)
    //         invoice.Status = InvoiceStatus.Partial;
    //     else
    //         invoice.Status = InvoiceStatus.Unpaid;

    //     // Save everything in a single transaction
    //     await _context.SaveChangesAsync();

    //     return true;
    // }

    public async Task<int> VoidOldInvoicesAsync(int daysThreshold = 10)
    {
        var thresholdDate = DateTime.UtcNow.AddDays(-daysThreshold);

        var invoicesToVoid = await _invoiceRepository.GetUnpaidOlderThanAsync(thresholdDate);

        foreach (var invoice in invoicesToVoid)
        {
            invoice.Status = InvoiceStatus.Void;
        }

        await _invoiceRepository.UpdateInvoicesAsync(invoicesToVoid);

        return invoicesToVoid.Count;
    }
    public async Task<List<InvoiceListDto>> SearchInvoicesAsync(int? customerId, InvoiceStatus? status, string? name)
    {
        var invoices = await _invoiceRepository.SearchInvoicesAsync(customerId, status, name);

        return invoices.Select(i => new InvoiceListDto
        {
            InvoiceId = i.InvoiceId,
            CustomerName = i.Booking.Customer?.Name ?? string.Empty,
            Amount = i.AmountDue,
            Status = i.Status.ToString(),
            IssueDate = i.IssueDate,
            DueDate = i.DueDate ?? DateTime.MinValue
        }).ToList();
    }
    // public async Task<IEnumerable<InvoiceDto>> GetUnpaidInvoicesOlderThanAsync(int days)
    // {
    //     var cutoff = DateTime.UtcNow.AddDays(-days);

    //     var invoices = await _context.Invoices
    //         .Include(i => i.Booking)
    //             .ThenInclude(b => b.Customer)
    //         .Include(i => i.Booking)
    //             .ThenInclude(b => b.Room)
    //         .Where(i => i.Status != InvoiceStatus.Paid && i.IssueDate < cutoff)
    //         .ToListAsync();

    //     return invoices.Select(MapToDto).ToList();
    // }

    // Helper mapping function
    private static InvoiceDto MapToDto(Invoice invoice)
    {
        return new InvoiceDto
        {
            InvoiceId = invoice.InvoiceId,
            AmountDue = invoice.AmountDue,
            IssueDate = invoice.IssueDate,
            Status = invoice.Status,
            BookingId = invoice.BookingId
        };
    }
    private string MapInvoiceStatus(InvoiceStatus status)
    {
        return status switch
        {
            InvoiceStatus.Paid => "Paid",
            InvoiceStatus.Unpaid => "Unpaid",
            InvoiceStatus.Void => "Void",
            InvoiceStatus.Partial => "Partial",
            _ => "Unknown"
        };
    }
    public InvoiceListDto MapToInvoiceListDto(Invoice invoice)
    {
        return new InvoiceListDto
        {
            InvoiceId = invoice.InvoiceId,
            CustomerName = invoice.Booking.Customer.Name,
            Amount = invoice.AmountDue,
            Status = MapInvoiceStatus(invoice.Status),
            IssueDate = invoice.IssueDate,
            DueDate = invoice.IssueDate.AddDays(10) // example: 2-week due date
        };
    }
}
