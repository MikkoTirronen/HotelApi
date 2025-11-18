using HotelApi.src.HotelApi.Core.Interfaces;
using HotelApi.src.HotelApi.Domain.DTOs;
using HotelApi.src.HotelApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HotelApi.src.HotelApi.Api.Endpoints;

public static class RoomEndpoints
{
    public static void MapRoomEndpoints(this WebApplication app)
    {

        var group = app.MapGroup("/rooms").WithTags("Rooms");

        group.MapGet("/", async (IRoomService service) =>
            Results.Ok(await service.GetAllRoomsAsync()));

        group.MapGet("/{id:int}", async (int id, IRoomService service) =>
        {
            var room = await service.GetRoomByIdAsync(id);
            return room != null ? Results.Ok(room) : Results.NotFound();
        });

        group.MapPost("/", async (CreateRoomDto room, IRoomService service) =>
        {
            var created = await service.CreateRoomAsync(room);
            return Results.Created($"/{created.Id}", created);
        });

        group.MapPut("/", async (int id, [FromBody] UpdateRoomDto room, IRoomService service) =>
        {
            var updated = await service.UpdateRoomAsync(id, room);
            return updated != null ? Results.Ok(updated) : Results.NotFound();
        });

        group.MapDelete("/{id:int}", async (int id, IRoomService service) =>
        {
            var deleted = await service.DeleteRoomAsync(id);
            return deleted ? Results.NoContent() : Results.NotFound();
        });

        group.MapGet("/rooms/available", async (
            [FromQuery] DateTime start,
            [FromQuery] DateTime end,
            [FromQuery] int guests,
            IRoomService roomService) =>
        {
            if (start >= end)
                return Results.BadRequest("End date must be after start date.");

            if (guests <= 0)
                return Results.BadRequest("Number of guests must be greater than zero.");

            var availableRooms = await roomService.GetAvailableRoomsAsync(start, end, guests);
            return Results.Ok(availableRooms);
        })
        .WithName("GetAvailableRooms")
        .WithOpenApi(); // Optional: shows nicely in Swagger
    }
    
}