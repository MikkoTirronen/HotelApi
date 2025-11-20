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

    // public async Task<Invoice?> GetInvoiceWithBookingsByIdAsync(int id)
    // {
    //     return await _context.Invoices
    //         .Include(i => i.Booking)
    //         .ThenInclude(b => b.Customer)
    //         .Include(i => i.Booking)
    //         .ThenInclude(b => b.Room)
    //         .FirstOrDefaultAsync(i => i.InvoiceId == id);
    // }

    public async Task<IEnumerable<Invoice>> GetAllWithIncludesAsync()
    {
        return await _context.Invoices
            .Include(i => i.Booking)
            .ThenInclude(b => b.Customer)
            .ToListAsync();
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
                        && i.IssueDate <= thresholdDate)
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
}