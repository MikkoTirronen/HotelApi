using HotelApi.src.HotelApi.Data.Interfaces;
using HotelApi.src.HotelApi.Domain.DTOs;
using HotelApi.src.HotelApi.Domain.Entities;

namespace HotelApi.src.HotelApi.Data.Interfaces;

public interface IRoomRepository : IGenericRepository<Room>
{
    Task<IEnumerable<RoomDto>> GetAllWithBookingsAsync();
    Task<RoomDto?> GetByIdWithBookingsAsync(int id);
}