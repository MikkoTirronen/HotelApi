using HotelApi.src.HotelApi.Core.Interfaces;
using HotelApi.src.HotelApi.Domain.DTOs;
using HotelApi.src.HotelApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HotelApi.src.HotelApi.Api.Endpoints;

public static class RoomEndpoints
{
    public static void MapRoomEndpoints(this WebApplication app)
    {
        app.MapGet("/rooms", async (IRoomService service) =>
            Results.Ok(await service.GetAllRoomsAsync()));

        app.MapGet("/rooms/{id:int}", async (int id, IRoomService service) =>
        {
            var room = await service.GetRoomByIdAsync(id);
            return room != null ? Results.Ok(room) : Results.NotFound();
        });

        app.MapPost("/rooms", async (CreateRoomDto room, IRoomService service) =>
        {
            var created = await service.CreateRoomAsync(room);
            return Results.Created($"/rooms/{created.Id}", created);
        });

        app.MapPut("/rooms", async (int id, [FromBody] UpdateRoomDto room, IRoomService service) =>
        {
            var updated = await service.UpdateRoomAsync(id, room);
            return updated != null ? Results.Ok(updated) : Results.NotFound();
        });

        app.MapDelete("/rooms/{id:int}", async (int id, IRoomService service) =>
        {
            var deleted = await service.DeleteRoomAsync(id);
            return deleted ? Results.NoContent() : Results.NotFound();
        });
    }
}