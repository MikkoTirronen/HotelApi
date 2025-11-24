using HotelApi.src.HotelApi.Domain.DTOs;
using HotelApi.src.HotelApi.Domain.Entities;
using HotelApi.src.HotelApi.Domain.Enums;

namespace HotelApi.src.HotelApi.Core.Interfaces;

public interface IBookingService
{
    Task<IEnumerable<BookingDto>> GetAllBookingsAsync();
    Task<BookingDto?> GetBookingByIdAsync(int id);
    Task<IEnumerable<RoomDto>> GetAvailableRoomsAsync(DateTime start, DateTime end, int guests);
    Task<BookingDto> CreateBookingAsync(CreateBookingWithCustomerDto dto);
    Task<BookingDto?> UpdateBookingAsync(int id, UpdateBookingDto updatedBooking);
    // Task<BookingDto?> UpdateBookingStatusAsync(int id, BookingStatus status);
    // Task<bool> CancelBookingAsync(int id);
    Task<bool> DeleteBookingAsync(int id);
    Task<IEnumerable<BookingDto>> AdvancedSearchAsync(
    string? customer,
    string? room,
    int? bookingId,
    DateTime? startDate,
    DateTime? endDate,
    int? guests);

}