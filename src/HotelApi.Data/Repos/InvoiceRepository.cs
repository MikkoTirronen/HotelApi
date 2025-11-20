using HotelApi.src.HotelApi.Data.Contexts;
using HotelApi.src.HotelApi.Data.Interfaces;
using HotelApi.src.HotelApi.Domain.Entities;
using HotelApi.src.HotelApi.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace HotelApi.src.HotelApi.Data.Repos;

public class InvoiceRepository : GenericRepository<Invoice>, IInvoiceRepository
{
    private readonly HotelDbContext _context;

    public InvoiceRepository(HotelDbContext context) : base(context)
    {
        _context = context;
    }
    public async Task<Invoice?> GetInvoiceWithCustomerAsync(int id)
    {
        return await _context.Invoices
            .Include(i => i.Booking)
                .ThenInclude(b => b.Customer)
            .FirstOrDefaultAsync(i => i.InvoiceId == id);
    }

    public async Task<Invoice?> GetInvoiceByBookingIdAsync(int bookingId)
    {
        return await _context.Invoices
            .Include(i => i.Booking)
                .ThenInclude(b => b.Customer)
            .Include(i => i.Booking)
                .ThenInclude(b => b.Room)
            .FirstOrDefaultAsync(i => i.BookingId == bookingId);
    }
    // public async Task<IEnumerable<Invoice>> GetInvoicesWithBookingsAsync()
    // {
    //     return await _context.Invoices
    //         .Include(i => i.Booking)
    //         .ThenInclude(b => b.Customer)
    //         .Include(i => i.Booking)
    //         .ThenInclude(b => b.Room)
    //         .ToListAsync();
    // }

    public async Task<Invoice?> GetByIdAsync(int id, bool includeBooking = false)
    {
        IQueryable<Invoice> query = _context.Invoices;

        if (includeBooking)
            query = query.Include(i => i.Booking);

        return await query.FirstOrDefaultAsync(i => i.InvoiceId == id);
    }

    public async Task<IEnumerable<Invoice>> GetAllWithIncludesAsync()
    {
        return await _context.Invoices
            .Include(i => i.Booking)
            .ThenInclude(b => b.Customer)
            .ToListAsync();
    }
    public async Task<Invoice?> GetInvoiceWithBookingAsync(int invoiceId)
    {
        return await _context.Invoices
            .Include(i => i.Booking)
                .ThenInclude(b => b.Customer)
            .Include(i => i.Payments)
            .FirstOrDefaultAsync(i => i.InvoiceId == invoiceId);
    }
    // public async Task<Invoice?> GetByIdWithIncludesAsync(int id)
    // {
    //     return await _context.Invoices
    //         .Include(i => i.Booking)
    //         .ThenInclude(b => b.Customer)
    //         .FirstOrDefaultAsync(i => i.InvoiceId == id);
    // }
    public async Task<List<Invoice>> GetUnpaidOlderThanAsync(DateTime thresholdDate)
    {
        return await _context.Invoices
            .Where(i => (i.Status == InvoiceStatus.Unpaid || i.Status == InvoiceStatus.Partial)
                        && i.IssueDate <= thresholdDate).Include(i => i.Booking)
            .ToListAsync();
    }

    public async Task UpdateInvoicesAsync(List<Invoice> invoices)
    {
        _context.Invoices.UpdateRange(invoices);
        await _context.SaveChangesAsync();
    }
    public async Task<Invoice?> GetInvoiceWithDetailsAsync(int id)
    {
        return await _context.Invoices
            .Include(i => i.Booking)
                .ThenInclude(b => b.Customer)
            .Include(i => i.Booking)
                .ThenInclude(b => b.Room)
            .FirstOrDefaultAsync(i => i.InvoiceId == id);
    }
    public async Task<List<Invoice>> SearchInvoicesAsync(int? customerId, InvoiceStatus? status, string? name)
    {
        var query = _context.Invoices
            .Include(i => i.Booking)
                .ThenInclude(b => b.Customer)
            .AsQueryable();

        if (customerId.HasValue)
            query = query.Where(i => i.Booking.Customer.CustomerId == customerId.Value);

        if (status.HasValue)
            query = query.Where(i => i.Status == status.Value);

        if (!string.IsNullOrWhiteSpace(name))
        {
            var search = $"%{name}%";
            query = query.Where(i =>
                EF.Functions.ILike(i.Booking.Customer.Name, search));
        }

        return await query
            .OrderByDescending(i => i.IssueDate)
            .ToListAsync();
    }
}