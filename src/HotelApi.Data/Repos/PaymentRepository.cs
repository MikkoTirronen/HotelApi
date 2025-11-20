using HotelApi.src.HotelApi.Data.Contexts;
using HotelApi.src.HotelApi.Data.Interfaces;
using HotelApi.src.HotelApi.Domain.Entities;

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
}