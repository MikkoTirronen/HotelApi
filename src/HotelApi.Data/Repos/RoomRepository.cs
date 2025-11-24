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
        };
    }
}