using HotelApi.src.HotelApi.Domain.Entities;

namespace HotelApi.src.HotelApi.Core.Interfaces;

public interface IRoomService
{
    Task<IEnumerable<Room>> GetAllRoomsAsync();
    Task<Room?> GetRoomByIdAsync(int id);
    Task<Room> CreateRoomAsync(Room room);
    Task<Room?> UpdateRoomAsync(int id, Room updatedRoom);
    Task<bool> DeleteRoomAsync(int id);
}