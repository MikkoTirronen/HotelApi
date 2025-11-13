using HotelApi.src.HotelApi.Domain.Entities;

namespace HotelApi.src.HotelApi.Core.Interfaces;

public interface IBookingService
{
    Task<IEnumerable<Booking>> GetAllBookingsAsync();
    Task<Booking?> GetBookingByIdAsync(int id);
    Task<IEnumerable<Room>> GetAvailableRoomsAsync(DateTime start, DateTime end, int guests);
    Task<Booking?> CreateBookingAsync(Booking booking);
    Task<Booking?> UpdateBookingAsync(int id, Booking updatedBooking);
    Task<bool> CancelBookingAsync(int id);
}