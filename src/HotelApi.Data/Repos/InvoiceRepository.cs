using HotelApi.src.HotelApi.Data.Contexts;
using HotelApi.src.HotelApi.Data.Interfaces;
using HotelApi.src.HotelApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelApi.src.HotelApi.Data.Repos;

public class InvoiceRepository : GenericRepository<Invoice>, IInvoiceRepository
{
    private readonly HotelDbContext _context;

    public InvoiceRepository(HotelDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Invoice>> GetInvoicesWithBookingsAsync()
    {
        return await _context.Invoices
            .Include(i => i.Booking)
            .ThenInclude(b => b.Customer)
            .Include(i => i.Booking)
            .ThenInclude(b => b.Room)
            .ToListAsync();
    }

    public async Task<Invoice?> GetInvoiceWithBookingsByIdAsync(int id)
    {
        return await _context.Invoices
            .Include(i => i.Booking)
            .ThenInclude(b => b.Customer)
            .Include(i => i.Booking)
            .ThenInclude(b => b.Room)
            .FirstOrDefaultAsync(i => i.InvoiceId == id);
    }
}