using HotelApi.src.HotelApi.Core.Interfaces;
using HotelApi.src.HotelApi.Data.Interfaces;
using HotelApi.src.HotelApi.Domain.DTOs;
using HotelApi.src.HotelApi.Domain.Entities;
using HotelApi.src.HotelApi.Domain.Enums;

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
            Status = BookingStatus.Pending
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
        if (!available.Any(r => r.RoomId == booking.RoomId))
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
        BookingId = b.BookingId,
        Customer = b.Customer == null ? new CustomerDto { } : new CustomerDto
        {
            CustomerId = b.Customer.CustomerId,
            Name = b.Customer.Name,
            Email = b.Customer.Email,
            Phone = b.Customer.Phone
        },
        Room = b.Room == null ? new RoomDto { } : new RoomDto
        {
            RoomId = b.Room.RoomId,
            RoomNumber = b.Room.RoomNumber,
            PricePerNight = b.Room.PricePerNight,
            BaseCapacity = b.Room.BaseCapacity,
            MaxExtraBeds = b.Room.MaxExtraBeds,
            Amenities = b.Room.Amenities,
            Active = b.Room.Active
        },
        Invoice = b.Invoice == null ? new InvoiceDto { } : new InvoiceDto
        {
            InvoiceId = b.Invoice.InvoiceId,
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
