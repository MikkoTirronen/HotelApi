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

    public async Task<BookingDto> CreateBookingAsync(CreateBookingDto dto)
    {
        var customer = await _customerRepository.GetByIdAsync(dto.CustomerId)
            ?? throw new Exception("Invalid Customer ID");

        var room = await _roomRepository.GetByIdAsync(dto.RoomId)
            ?? throw new Exception("Invalid Room ID");

        // Check availability
        var available = await GetAvailableRoomsAsync(dto.StartDate, dto.EndDate, dto.NumPersons);
        if (!available.Any(r => r.Id == dto.RoomId))
            throw new Exception("Room not available for selected dates");

        int nights = (dto.EndDate - dto.StartDate).Days;
        decimal total = room.PricePerNight * nights;

        var booking = new Booking
        {
            CustomerId = dto.CustomerId,
            RoomId = dto.RoomId,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            NumPersons = dto.NumPersons,
            TotalPrice = total,
            Status = BookingStatus.Pending
        };

        await _bookingRepository.AddAsync(booking);
        await _bookingRepository.SaveAsync();

        await _invoiceService.CreateInvoiceAsync(booking.Id, total);

        var fullBooking = await _bookingRepository.GetByIdWithIncludesAsync(booking.Id);
        return MapToDto(fullBooking!);
    }



    public async Task<BookingDto?> UpdateBookingAsync(int id, UpdateBookingDto dto)
{
    var booking = await _bookingRepository.GetByIdWithIncludesAsync(id);
    if (booking == null) return null;

    if (dto.RoomId.HasValue)
        booking.RoomId = dto.RoomId.Value;

    if (dto.StartDate.HasValue)
        booking.StartDate = dto.StartDate.Value;

    if (dto.EndDate.HasValue)
        booking.EndDate = dto.EndDate.Value;

    if (dto.NumPersons.HasValue)
        booking.NumPersons = dto.NumPersons.Value;

    // Validate availability on update
    var available = await GetAvailableRoomsAsync(booking.StartDate, booking.EndDate, booking.NumPersons);
    if (!available.Any(r => r.Id == booking.RoomId))
        throw new Exception("Room not available for updated dates");

    // Recalculate total
    var room = await _roomRepository.GetByIdAsync(booking.RoomId);
    if (room != null)
        booking.TotalPrice = room.PricePerNight * (booking.EndDate - booking.StartDate).Days;

     _bookingRepository.Update(booking);
    await _bookingRepository.SaveAsync();

    return MapToDto(booking);
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
        Status = b.Status
    };
}
