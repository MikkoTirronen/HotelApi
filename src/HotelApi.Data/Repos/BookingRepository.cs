using HotelApi.src.HotelApi.Data.Contexts;
using HotelApi.src.HotelApi.Data.Interfaces;
using HotelApi.src.HotelApi.Domain.Entities;
using HotelApi.src.HotelApi.Domain.Enums;
using Microsoft.EntityFrameworkCore;

public class BookingRepository : IBookingRepository
{
    private readonly HotelDbContext _context;

    public BookingRepository(HotelDbContext context)
    {
        _context = context;
    }

    public async Task<List<Booking>> GetAllWithIncludesAsync(bool includeRoom = true, bool includeCustomer = true, bool includeInvoice = true)
    {
        IQueryable<Booking> query = _context.Bookings;

        if (includeRoom) query = query.Include(b => b.Room);
        if (includeCustomer) query = query.Include(b => b.Customer);
        if (includeInvoice) query = query.Include(b => b.Invoice);

        return await query.ToListAsync();
    }

    public async Task<Booking?> GetByIdWithIncludesAsync(int id, bool includeRoom = true, bool includeCustomer = true, bool includeInvoice = true)
    {
        IQueryable<Booking> query = _context.Bookings;

        if (includeRoom) query = query.Include(b => b.Room);
        if (includeCustomer) query = query.Include(b => b.Customer);
        if (includeInvoice) query = query.Include(b => b.Invoice);

        return await query.FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<List<Booking>> GetBookingsInDateRangeAsync(DateTime start, DateTime end) =>
    await _context.Bookings
        .Where(b => b.StartDate < end && b.EndDate > start && b.Status != BookingStatus.Canceled)
        .Include(b => b.Room)
        .Include(b => b.Customer)
        .ToListAsync();

    public async Task AddAsync(Booking booking) => await _context.Bookings.AddAsync(booking);
    public void Update(Booking booking)
        => _context.Bookings.Update(booking);

    public async Task SaveAsync() => await _context.SaveChangesAsync();
}
