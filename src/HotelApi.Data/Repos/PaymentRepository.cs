using HotelApi.src.HotelApi.Data.Contexts;
using HotelApi.src.HotelApi.Data.Interfaces;
using HotelApi.src.HotelApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelApi.src.HotelApi.Data.Repos;

public class PaymentRepository : IPaymentRepository
{
    private readonly HotelDbContext _context;

    public PaymentRepository(HotelDbContext context)
    {
        _context = context;
    }

    public async Task AddPaymentAsync(PaymentRecord payment)
    {
        await _context.Payments.AddAsync(payment);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<PaymentRecord>> GetAllPaymentsOrderedAsync()
    {
        return await _context.Payments
            .AsNoTracking()
            .Include(p => p.Invoice)
                .ThenInclude(i => i.Booking)
                    .ThenInclude(b => b.Customer)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync();
    }
}