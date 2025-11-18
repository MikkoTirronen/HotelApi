using HotelApi.src.HotelApi.Core.Interfaces;
using HotelApi.src.HotelApi.Data.Interfaces;
using HotelApi.src.HotelApi.Domain.DTOs;
using HotelApi.src.HotelApi.Domain.Entities;

namespace HotelApi.src.HotelApi.Core.Services;

public class RoomService(IRoomRepository roomRepository) : IRoomService
{
    private readonly IRoomRepository _roomRepository = roomRepository;

    public async Task<IEnumerable<RoomDto>> GetAllRoomsAsync() => await _roomRepository.GetAllWithBookingsAsync();
    public async Task<RoomDto?> GetRoomByIdAsync(int id) => await _roomRepository.GetByIdWithBookingsAsync(id);
    public async Task<RoomDto> CreateRoomAsync(CreateRoomDto dto)
    {
        var room = new Room
        {
            RoomNumber = dto.RoomNumber,
            PricePerNight = dto.PricePerNight,
            BaseCapacity = dto.BaseCapacity,
            MaxExtraBeds = dto.MaxExtraBeds,
            Amenities = dto.Amenities,
            Active = true
        };
        await _roomRepository.AddAsync(room);
        await _roomRepository.SaveAsync();
        return new RoomDto
        {
            Id = room.Id,
            RoomNumber = room.RoomNumber,
            PricePerNight = room.PricePerNight,
            BaseCapacity = room.BaseCapacity,
            MaxExtraBeds = room.MaxExtraBeds,
            Amenities = room.Amenities,
            Active = room.Active
           // Bookings = [] // or new List<BookingDto>()
        };
    }
    public async Task<RoomDto?> UpdateRoomAsync(int id, UpdateRoomDto dto)
    {
        var room = await _roomRepository.GetByIdAsync(id);
        if (room == null)
            return null;

        // Update only allowed fields
        if (dto.PricePerNight.HasValue)
            room.PricePerNight = dto.PricePerNight.Value;

        if (dto.BaseCapacity.HasValue)
            room.BaseCapacity = dto.BaseCapacity.Value;

        if (dto.MaxExtraBeds.HasValue)
            room.MaxExtraBeds = dto.MaxExtraBeds.Value;

        if (!string.IsNullOrWhiteSpace(dto.Amenities))
            room.Amenities = dto.Amenities;

        if (dto.Active.HasValue)
            room.Active = dto.Active.Value;

        _roomRepository.Update(room);
        await _roomRepository.SaveAsync();

        var roomDto = new RoomDto
        {
            Id = room.Id,
            RoomNumber = room.RoomNumber,
            PricePerNight = room.PricePerNight,
            BaseCapacity = room.BaseCapacity,
            MaxExtraBeds = room.MaxExtraBeds,
            Amenities = room.Amenities,
            Active = room.Active,
            // Bookings = room.Bookings.Select(b => new BookingDto
            // {
            //     Id = b.Id,
            //     Customer = new CustomerDto
            //     {
            //         Id = b.Customer.Id,
            //         FirstName = b.Customer.FirstName,
            //         LastName = b.Customer.LastName,
            //         Email = b.Customer.Email,
            //         Phone = b.Customer.Phone
            //     },
            //     Invoice = b.Invoice == null ? new InvoiceDto { } : new InvoiceDto
            //     {
            //         Id = b.Invoice.Id,
            //         AmountDue = b.Invoice.AmountDue,
            //         Status = b.Invoice.Status,
            //         IssueDate = b.Invoice.IssueDate
            //     },
            //     NumPersons = b.NumPersons,
            //     StartDate = b.StartDate,
            //     EndDate = b.EndDate,
            //     Status = (Domain.Enums.InvoiceStatus)b.Status
            // }
            // ).ToList()
        };

        return roomDto;
    }

    public async Task<bool> DeleteRoomAsync(int id)
    {
        var room = await _roomRepository.GetByIdAsync(id);
        if (room == null) return false;

        _roomRepository.Delete(room);
        await _roomRepository.SaveAsync();
        return true;
    }

}