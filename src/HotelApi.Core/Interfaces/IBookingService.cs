using HotelApi.src.HotelApi.Domain.DTOs;
using HotelApi.src.HotelApi.Domain.Entities;
using HotelApi.src.HotelApi.Domain.Enums;

namespace HotelApi.src.HotelApi.Core.Interfaces;

public interface IBookingService
{
    Task<IEnumerable<BookingDto>> GetAllBookingsAsync();
    Task<BookingDto?> GetBookingByIdAsync(int id);
    Task<IEnumerable<Room>> GetAvailableRoomsAsync(DateTime start, DateTime end, int guests);
    Task<BookingDto> CreateBookingAsync(Booking booking);
    Task<BookingDto?> UpdateBookingAsync(int id, Booking updatedBooking);
    // Task<BookingDto?> UpdateBookingStatusAsync(int id, BookingStatus status);
    Task<bool> CancelBookingAsync(int id);
}