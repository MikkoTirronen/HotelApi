using HotelApi.src.HotelApi.Core.Interfaces;
using HotelApi.src.HotelApi.Data.Interfaces;
using HotelApi.src.HotelApi.Domain.Entities;
using HotelApi.src.HotelApi.Domain.Enums;

namespace HotelApi.src.HotelApi.Core.Services;

public class BookingService(
    IGenericRepository<Booking> bookingRepository,
    IGenericRepository<Room> roomRepository,
    IGenericRepository<Customer> customerRepository,
    IInvoiceService invoiceService
    ) : IBookingService
{
    private readonly IGenericRepository<Booking> _bookingRepository = bookingRepository;
    private readonly IGenericRepository<Room> _roomRepository = roomRepository;
    private readonly IGenericRepository<Customer> _customerRepository = customerRepository;
    private readonly IInvoiceService _invoiceService = invoiceService;

    public async Task<IEnumerable<Booking>> GetAllBookingsAsync() => await _bookingRepository.GetAllAsync();
    public async Task<Booking?> GetBookingByIdAsync(int id) => await _bookingRepository.GetByIdAsync(id);
    public async Task<IEnumerable<Room>> GetAvailableRoomsAsync(DateTime start, DateTime end, int guests)
    {
        var allRooms = await _roomRepository.GetAllAsync();
        var allBookings = await _bookingRepository.GetAllAsync();

        var availableRooms = allRooms
            .Where(room =>
                room.Active &&
                (room.BaseCapacity + room.MaxExtraBeds) >= guests &&
                !allBookings.Any(b =>
                    b.RoomId == room.Id &&
                    b.Status == BookingStatus.Pending &&
                    b.StartDate < end &&
                    b.EndDate > start))
            .ToList();

        return availableRooms;
    }

    public async Task<Booking?> CreateBookingAsync(Booking booking)
    {
        var customer = await _customerRepository.GetByIdAsync(booking.CustomerId) 
            ?? throw new Exception("Invalid Customer ID");
        var room = await _roomRepository.GetByIdAsync(booking.RoomId)
            ?? throw new Exception("Invalid or inactive room");

        var availableRooms = await GetAvailableRoomsAsync(booking.StartDate, booking.EndDate, booking.NumPersons);

        if (!availableRooms.Any(r => r.Id == booking.RoomId))
            throw new Exception("Room is not available for selected dates");
            
        var nights = (booking.EndDate - booking.StartDate).Days;
        booking.TotalPrice = room.PricePerNight * nights;

        await _bookingRepository.AddAsync(booking);
        await _bookingRepository.SaveAsync();

        await _invoiceService.CreateInvoiceAsync(booking.Id, booking.TotalPrice);

        return booking;
    }

    public async Task<Booking?> UpdateBookingAsync(int id, Booking updatedBooking)
    {
        var existing = await _bookingRepository.GetByIdAsync(id);
        if (existing == null) return null;

        var availableRooms = await GetAvailableRoomsAsync(updatedBooking.StartDate, updatedBooking.EndDate, updatedBooking.NumPersons);

        if (!availableRooms.Any(r => r.Id == updatedBooking.RoomId))
            throw new Exception("Room is not available for the updated dates");

        existing.RoomId = updatedBooking.RoomId;
        existing.StartDate = updatedBooking.StartDate;
        existing.EndDate = updatedBooking.EndDate;
        existing.NumPersons = updatedBooking.NumPersons;

        var room = await _roomRepository.GetByIdAsync(updatedBooking.RoomId);

        if (room != null)
            existing.TotalPrice = room.PricePerNight * (updatedBooking.EndDate - updatedBooking.StartDate).Days;

        _bookingRepository.Update(existing);
        await _bookingRepository.SaveAsync();
        return existing;
    }
    
    public async Task<bool> CancelBookingAsync(int id)
    {
        var booking = await _bookingRepository.GetByIdAsync(id);
        if (booking == null) return false;

        booking.Status = BookingStatus.Canceled;
        _bookingRepository.Update(booking);
        await _bookingRepository.SaveAsync();
        return true;
    }
}