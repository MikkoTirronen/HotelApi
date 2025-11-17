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
    public async Task<Room> CreateRoomAsync(Room room)
    {
        await _roomRepository.AddAsync(room);
        await _roomRepository.SaveAsync();
        return room;
    }
    public async Task<Room?> UpdateRoomAsync(int id, Room updatedRoom)
    {
        var existing = await _roomRepository.GetByIdAsync(id);
        if (existing == null) return null;

        existing.RoomNumber = updatedRoom.RoomNumber;
        existing.Type = updatedRoom.Type;
        existing.BaseCapacity = updatedRoom.BaseCapacity;
        existing.MaxExtraBeds = updatedRoom.MaxExtraBeds;
        existing.PricePerNight = updatedRoom.PricePerNight;
        existing.Amenities = updatedRoom.Amenities;
        existing.Active = updatedRoom.Active;

        _roomRepository.Update(existing);
        await _roomRepository.SaveAsync();
        return existing;
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