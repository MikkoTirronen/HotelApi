using HotelApi.src.HotelApi.Core.Interfaces;
using HotelApi.src.HotelApi.Data.Interfaces;
using HotelApi.src.HotelApi.Domain.DTOs;
using HotelApi.src.HotelApi.Domain.Entities;
using HotelApi.src.HotelApi.Domain.Enums;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HotelApi.src.HotelApi.Core.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IGenericRepository<Room> _roomRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IInvoiceService _invoiceService;

    public BookingService(
        IBookingRepository bookingRepository,
        IGenericRepository<Room> roomRepository,
        ICustomerRepository customerRepository,
        IInvoiceService invoiceService)
    {
        _bookingRepository = bookingRepository;
        _roomRepository = roomRepository;
        _customerRepository = customerRepository;
        _invoiceService = invoiceService;
    }

    public async Task<IEnumerable<BookingDto>> GetAllBookingsAsync()
    {
        var bookings = await _bookingRepository.GetAllWithIncludesAsync();
        return bookings.Select(MapToDto).ToList();
    }

    public async Task<BookingDto?> GetBookingByIdAsync(int id)
    {
        var booking = await _bookingRepository.GetByIdWithIncludesAsync(id);
        return booking == null ? null : MapToDto(booking);
    }

    public async Task<IEnumerable<RoomDto>> GetAvailableRoomsAsync(DateTime start, DateTime end, int guests)
    {
        var allRooms = await _roomRepository.GetAllAsync();
        var overlappingBookings = await _bookingRepository.GetBookingsInDateRangeAsync(start, end);

        var availableRooms = allRooms
            .Where(room =>
                room.Active &&
                (room.BaseCapacity + room.MaxExtraBeds) >= guests &&
                !overlappingBookings.Any(b => b.Room.RoomId == room.RoomId))
            .ToList();

        return availableRooms.Select(r => new RoomDto
        {
            RoomId = r.RoomId,
            RoomNumber = r.RoomNumber,
            PricePerNight = r.PricePerNight,
            BaseCapacity = r.BaseCapacity,
            MaxExtraBeds = r.MaxExtraBeds,
            Amenities = r.Amenities,
            Active = r.Active
        });
    }

    public async Task<BookingDto> CreateBookingAsync(CreateBookingWithCustomerDto dto)
    {
        // 1. Find or create customer
        var customer = await _customerRepository.GetByEmailAsync(dto.Email);

        if (customer == null)
        {
            customer = new Customer
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _customerRepository.AddAsync(customer);
            await _customerRepository.SaveAsync(); // 🔥 customer.Id is generated here
        }


        // 2. Validate room
        var room = await _roomRepository.GetByIdAsync(dto.RoomId)
            ?? throw new Exception("Invalid Room ID");

        // 3. Check availability
        var available = await GetAvailableRoomsAsync(dto.StartDate, dto.EndDate, dto.NumPersons);
        if (!available.Any(r => r.RoomId == dto.RoomId))
            throw new Exception("Room not available for selected dates");

        // Calculate total price
        int nights = (dto.EndDate - dto.StartDate).Days;
        decimal total = room.PricePerNight * nights;

        // 4. Create booking
        var booking = new Booking
        {
            CustomerId = customer.CustomerId,  // 🔥 USE THE GENERATED ID
            RoomId = dto.RoomId,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            NumPersons = dto.NumPersons,
            Status = BookingStatus.Pending,
            TotalPrice = total,
            ExtraBedsCount = dto.ExtraBedsCount ?? 0,
        };

        await _bookingRepository.AddAsync(booking);
        await _bookingRepository.SaveAsync();

        // 5. Create invoice
        await _invoiceService.CreateInvoiceAsync(booking.BookingId, total);

        // 6. Fetch booking with full navigation properties
        var fullBooking = await _bookingRepository.GetByIdWithIncludesAsync(booking.BookingId);

        return MapToDto(fullBooking!);
    }

    public async Task<IEnumerable<BookingDto>> AdvancedSearchAsync(
        string? customer,
        string? room,
        int? bookingId,
        DateTime? startDate,
        DateTime? endDate,
        int? guests)
    {
        var results = await _bookingRepository.AdvancedSearchAsync(
            customer, room, bookingId, startDate, endDate, guests);

        return results.Select(MapToDto);
    }



    public async Task<BookingDto?> UpdateBookingAsync(int bookingId, UpdateBookingDto dto)
    {
        var booking = await _bookingRepository.GetByIdWithIncludesAsync(bookingId)
            ?? throw new Exception("Booking not found");

        // Check availability (ignore the booking itself)
        bool available = await IsRoomAvailableForUpdateAsync(
            dto.RoomId,
            dto.StartDate,
            dto.EndDate,
            bookingId
        );

        if (!available)
            throw new Exception("Room not available for updated dates");

        // Update fields
        var selectedRoom = await _roomRepository.GetByIdAsync(dto.RoomId);
        booking.RoomId = dto.RoomId;
        booking.StartDate = dto.StartDate;
        booking.EndDate = dto.EndDate;
        booking.NumPersons = dto.NumPersons ?? 0;
        booking.TotalPrice = CalculatePrice(selectedRoom, dto.StartDate, dto.EndDate);
        booking.ExtraBedsCount = dto.ExtraBedsCount ?? 0;
        await _bookingRepository.SaveAsync();

        var full = await _bookingRepository.GetByIdWithIncludesAsync(bookingId);
        return MapToDto(full!);
    }

    private static decimal CalculatePrice(Room? room, DateTime start, DateTime end)
    {
        int nights = (end - start).Days;
        if (nights <= 0) nights = 1; // fallback to at least 1 night
        if (room != null)
            return room.PricePerNight * nights;
        return 0;
    }

    public async Task<bool> CancelBookingAsync(int id)
    {
        var booking = await _bookingRepository.GetByIdWithIncludesAsync(id);
        if (booking == null) return false;

        booking.Status = BookingStatus.Canceled;
        _bookingRepository.Update(booking);
        await _bookingRepository.SaveAsync();
        return true;
    }
    public async Task<bool> DeleteBookingAsync(int id)
    {
        // Fetch the booking from the database
        var booking = await _bookingRepository.GetByIdWithIncludesAsync(id);
        if (booking == null) return false; // Nothing to delete

        // Remove the booking from the DbContext
        await _bookingRepository.DeleteBookingAsync(id);
        return true;
    }
    public async Task<bool> IsRoomAvailableForUpdateAsync(int roomId, DateTime start, DateTime end, int bookingId)
    {
        var overlapping = await _bookingRepository.GetBookingsInDateRangeAsync(start, end);

        return !overlapping
            .Any(b => b.RoomId == roomId && b.BookingId != bookingId);
    }


    // Map Booking -> BookingDto
    public BookingDto MapToDto(Booking b)
    {
        return new BookingDto
        {
            BookingId = b.BookingId,
            RoomId = b.RoomId,
            CustomerId = b.CustomerId,
            StartDate = b.StartDate,
            EndDate = b.EndDate,
            NumPersons = b.NumPersons,
            TotalPrice = b.TotalPrice,
            Status = b.Status,
            ExtraBedsCount = b.ExtraBedsCount,
            Room = new RoomDto
            {
                RoomId = b.Room.RoomId,
                RoomNumber = b.Room.RoomNumber,
                PricePerNight = b.Room.PricePerNight,
                BaseCapacity = b.Room.BaseCapacity,
                MaxExtraBeds = b.Room.MaxExtraBeds,
                Amenities = b.Room.Amenities,
                Active = b.Room.Active
            },

            Customer = new CustomerDto
            {
                CustomerId = b.Customer.CustomerId,
                Name = b.Customer.Name,
                Email = b.Customer.Email,
                Phone = b.Customer.Phone
            },

            Invoice = b.Invoice == null
                ? null
                : new InvoiceDto
                {
                    InvoiceId = b.Invoice.InvoiceId,
                    BookingId = b.Invoice.BookingId,
                    AmountDue = b.Invoice.AmountDue,
                    IssueDate = b.Invoice.IssueDate,
                    Status = b.Invoice.Status
                }
        };
    }

}
