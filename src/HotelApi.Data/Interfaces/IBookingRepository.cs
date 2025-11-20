using HotelApi.src.HotelApi.Domain.Entities;

namespace HotelApi.src.HotelApi.Data.Interfaces;
public interface IBookingRepository
{
    Task<List<Booking>> GetAllWithIncludesAsync(bool includeRoom = true, bool includeCustomer = true, bool includeInvoice = true);
    Task<Booking?> GetByIdWithIncludesAsync(int id, bool includeRoom = true, bool includeCustomer = true, bool includeInvoice = true);
    Task<List<Booking>> GetBookingsInDateRangeAsync(DateTime start, DateTime end);
    Task AddAsync(Booking booking);
    void Update(Booking booking);
    Task SaveAsync();
    Task<List<Booking>> AdvancedSearchAsync(
    string? customer,
    string? room,
    int? bookingId,
    DateTime? startDate,
    DateTime? endDate,
    int? guests);
    Task<bool> DeleteBookingAsync(int id);
}   
