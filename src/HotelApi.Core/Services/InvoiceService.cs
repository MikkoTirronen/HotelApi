using HotelApi.src.HotelApi.Core.Interfaces;
using HotelApi.src.HotelApi.Data.Contexts;
using HotelApi.src.HotelApi.Domain.DTOs;
using HotelApi.src.HotelApi.Domain.Entities;
using HotelApi.src.HotelApi.Domain.Enums;
using Microsoft.EntityFrameworkCore;

public class InvoiceService : IInvoiceService
{
    private readonly HotelDbContext _context;

    public InvoiceService(HotelDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<InvoiceDto>> GetAllInvoicesAsync()
    {
        var invoices = await _context.Invoices
            .Include(i => i.Booking)
                .ThenInclude(b => b.Customer)
            .Include(i => i.Booking)
                .ThenInclude(b => b.Room)
            .ToListAsync();

        return invoices.Select(MapToDto).ToList();
    }

    public async Task<InvoiceDto?> GetInvoiceByIdAsync(int id)
    {
        var invoice = await _context.Invoices
            .Include(i => i.Booking)
                .ThenInclude(b => b.Customer)
            .Include(i => i.Booking)
                .ThenInclude(b => b.Room)
            .FirstOrDefaultAsync(i => i.InvoiceId == id);

        return invoice == null ? null : MapToDto(invoice);
    }

    public async Task<InvoiceDto?> GetInvoiceByBookingIdAsync(int bookingId)
    {
        var invoice = await _context.Invoices
            .Include(i => i.Booking)
                .ThenInclude(b => b.Customer)
            .Include(i => i.Booking)
                .ThenInclude(b => b.Room)
            .FirstOrDefaultAsync(i => i.BookingId == bookingId);

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

    public async Task<bool> RegisterPaymentAsync(int invoiceId, decimal amountPaid, string paymentMethod)
    {
        var invoice = await _context.Invoices
            .Include(i => i.Booking)
            .FirstOrDefaultAsync(i => i.InvoiceId == invoiceId);

        if (invoice == null) return false;

        // Create a payment record
        var payment = new PaymentRecord
        {
            InvoiceId = invoice.InvoiceId,
            AmountPaid = amountPaid,
            PaymentDate = DateTime.UtcNow,
            PaymentMethod = paymentMethod
        };

        await _context.Payments.AddAsync(payment);

        // Update invoice status
        invoice.Status = invoice.AmountDue <= amountPaid ? InvoiceStatus.Paid : InvoiceStatus.Unpaid;

        _context.Invoices.Update(invoice);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<InvoiceDto>> GetUnpaidInvoicesOlderThanAsync(int days)
    {
        var cutoff = DateTime.UtcNow.AddDays(-days);

        var invoices = await _context.Invoices
            .Include(i => i.Booking)
                .ThenInclude(b => b.Customer)
            .Include(i => i.Booking)
                .ThenInclude(b => b.Room)
            .Where(i => i.Status != InvoiceStatus.Paid && i.IssueDate < cutoff)
            .ToListAsync();

        return invoices.Select(MapToDto).ToList();
    }

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
            // Booking = invoice.Booking == null ? null : new BookingDto
            // {
            //     Id = invoice.Booking.Id,
            //     StartDate = invoice.Booking.StartDate,
            //     EndDate = invoice.Booking.EndDate,
            //     NumPersons = invoice.Booking.NumPersons,
            //     Status = (InvoiceStatus)invoice.Booking.Status,
            //     Customer = new CustomerDto
            //     {
            //         Id = invoice.Booking.Customer.Id,
            //         FirstName = invoice.Booking.Customer.FirstName,
            //         LastName = invoice.Booking.Customer.LastName,
            //         Email = invoice.Booking.Customer.Email,
            //         Phone = invoice.Booking.Customer.Phone
            //     },
            //     Room = new RoomDto
            //     {
            //         Id = invoice.Booking.Room.Id,
            //         RoomNumber = invoice.Booking.Room.RoomNumber,
            //         PricePerNight = invoice.Booking.Room.PricePerNight,
            //         BaseCapacity = invoice.Booking.Room.BaseCapacity,
            //         MaxExtraBeds = invoice.Booking.Room.MaxExtraBeds,
            //         Amenities = invoice.Booking.Room.Amenities
            //     }
            // }
        };
    }
}
