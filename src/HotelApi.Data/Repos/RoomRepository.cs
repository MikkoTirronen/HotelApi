using HotelApi.src.HotelApi.Data.Contexts;
using HotelApi.src.HotelApi.Data.Interfaces;
using HotelApi.src.HotelApi.Domain.DTOs;
using HotelApi.src.HotelApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelApi.src.HotelApi.Data.Repos;

public class RoomRepository : GenericRepository<Room>, IRoomRepository
{
    private readonly HotelDbContext _context;
    public RoomRepository(HotelDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RoomDto>> GetAllWithBookingsAsync()
    {
        var rooms = await _context.Rooms
            .Include(r => r.Bookings)
                .ThenInclude(b => b.Customer)
            .Include(r => r.Bookings)
                .ThenInclude(b => b.Invoice)
            .ToListAsync();

        return rooms.Select(r => new RoomDto
        {
            RoomId = r.RoomId,
            RoomNumber = r.RoomNumber,
            PricePerNight = r.PricePerNight,
            BaseCapacity = r.BaseCapacity,
            MaxExtraBeds = r.MaxExtraBeds,
            Amenities = r.Amenities,
            Active = r.Active,
            // Bookings = r.Bookings.Select(b => new BookingDto
            // {
            //     Id = b.Id,
            //     StartDate = b.StartDate,
            //     EndDate = b.EndDate,
            //     NumPersons = b.NumPersons,
            //     TotalPrice = b.TotalPrice,
            //     Status = (Domain.Enums.InvoiceStatus)b.Status,

            //     Customer = new CustomerDto
            //     {
            //         Id = b.Customer.Id,
            //         FirstName = b.Customer.FirstName,
            //         LastName = b.Customer.LastName,
            //         Email = b.Customer.Email,
            //         Phone = b.Customer.Phone,
            //         Address = b.Customer.Address
            //     },

            //     Invoice = b.Invoice == null ? new InvoiceDto() : new InvoiceDto
            //     {
            //         Id = b.Invoice.Id,
            //         AmountDue = b.Invoice.AmountDue,
            //         IssueDate = b.Invoice.IssueDate,
            //         Status = b.Invoice.Status
            //     }

            // }).ToList()
        });
    }
    public async Task<RoomDto?> GetByIdWithBookingsAsync(int id)
    {
        var room = await _context.Rooms
            .Include(r => r.Bookings)
                .ThenInclude(b => b.Customer)
            .Include(r => r.Bookings)
                .ThenInclude(b => b.Invoice)
            .FirstOrDefaultAsync(r => r.RoomId == id);

        if (room == null)
            return null;

        return new RoomDto
        {
            RoomId = room.RoomId,
            RoomNumber = room.RoomNumber,
            PricePerNight = room.PricePerNight,
            BaseCapacity = room.BaseCapacity,
            MaxExtraBeds = room.MaxExtraBeds,
            Amenities = room.Amenities,
            Active = room.Active,
            // Bookings = room.Bookings.Select(b => new BookingDto
            // {
            //     Id = b.Id,
            //     StartDate = b.StartDate,
            //     EndDate = b.EndDate,
            //     NumPersons = b.NumPersons,
            //     TotalPrice = b.TotalPrice,
            //     Status = (Domain.Enums.InvoiceStatus)b.Status,

            //     Customer = new CustomerDto
            //     {
            //         Id = b.Customer.Id,
            //         FirstName = b.Customer.FirstName,
            //         LastName = b.Customer.LastName,
            //         Email = b.Customer.Email,
            //         Phone = b.Customer.Phone,
            //         Address = b.Customer.Address
            //     },

            //     Invoice = b.Invoice == null ? new InvoiceDto() : new InvoiceDto
            //     {
            //         Id = b.Invoice.Id,
            //         AmountDue = b.Invoice.AmountDue,
            //         IssueDate = b.Invoice.IssueDate,
            //         Status = b.Invoice.Status
            //     }

            // }).ToList()
        };
    }
}