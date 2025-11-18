using HotelApi.src.HotelApi.Core.Interfaces;
using HotelApi.src.HotelApi.Data.Interfaces;
using HotelApi.src.HotelApi.Domain.DTOs;
using HotelApi.src.HotelApi.Domain.Entities;
using HotelApi.src.HotelApi.Domain.Enums;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IGenericRepository<Room> _roomRepository;
    private readonly IGenericRepository<Customer> _customerRepository;
    private readonly IInvoiceService _invoiceService;

    public BookingService(
        IBookingRepository bookingRepository,
        IGenericRepository<Room> roomRepository,
        IGenericRepository<Customer> customerRepository,
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

    public async Task<IEnumerable<Room>> GetAvailableRoomsAsync(DateTime start, DateTime end, int guests)
    {
        var allRooms = await _roomRepository.GetAllAsync();
        var overlappingBookings = await _bookingRepository.GetBookingsInDateRangeAsync(start, end);

        var availableRooms = allRooms
            .Where(room =>
                room.Active &&
                (room.BaseCapacity + room.MaxExtraBeds) >= guests &&
                !overlappingBookings.Any(b => b.RoomId == room.Id))
            .ToList();

        return availableRooms;
    }

    public async Task<BookingDto> CreateBookingAsync(Booking booking)
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

        // Fetch booking with includes for returning DTO
        var createdBooking = await _bookingRepository.GetByIdWithIncludesAsync(booking.Id);
        return MapToDto(createdBooking!);
    }

    public async Task<BookingDto?> UpdateBookingAsync(int id, Booking updatedBooking)
    {
        var existing = await _bookingRepository.GetByIdWithIncludesAsync(id);
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

        return MapToDto(existing);
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



    // Map Booking -> BookingDto
    private BookingDto MapToDto(Booking b) => new BookingDto
    {
        Id = b.Id,
        Customer = b.Customer == null ? new CustomerDto { } : new CustomerDto
        {
            Id = b.Customer.Id,
            FirstName = b.Customer.FirstName,
            LastName = b.Customer.LastName,
            Email = b.Customer.Email,
            Phone = b.Customer.Phone
        },
        Room = b.Room == null ? new RoomDto { } : new RoomDto
        {
            Id = b.Room.Id,
            RoomNumber = b.Room.RoomNumber,
            PricePerNight = b.Room.PricePerNight,
            BaseCapacity = b.Room.BaseCapacity,
            MaxExtraBeds = b.Room.MaxExtraBeds,
            Amenities = b.Room.Amenities,
            Active = b.Room.Active
        },
        Invoice = b.Invoice == null ? new InvoiceDto { } : new InvoiceDto
        {
            Id = b.Invoice.Id,
            AmountDue = b.Invoice.AmountDue,
            Status = b.Invoice.Status,
            IssueDate = b.Invoice.IssueDate
        },
        NumPersons = b.NumPersons,
        StartDate = b.StartDate,
        EndDate = b.EndDate,
        Status = (InvoiceStatus)b.Status
    };
}
