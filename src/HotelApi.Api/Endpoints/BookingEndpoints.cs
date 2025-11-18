using HotelApi.src.HotelApi.Core.Interfaces;
using HotelApi.src.HotelApi.Domain.Entities;

namespace HotelApi.src.HotelApi.Api.Endpoints;

public static class BookingEndPoints
{
    public static void MapBookingEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("bookings").WithTags("Bookings");

        group.MapGet("/", async (IBookingService service) =>
            Results.Ok(await service.GetAllBookingsAsync()));

        group.MapGet("/{id:int}", async (int id, IBookingService service) =>
        {
            var booking = await service.GetBookingByIdAsync(id);
            return booking != null ? Results.Ok(booking) : Results.NotFound();
        });

        group.MapPost("", async (Booking booking, IBookingService service) =>
        {
            try
            {
                var created = await service.CreateBookingAsync(booking);
                return Results.Created($"/{created?.Id}", created);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        });

        group.MapPut("/{id:int}", async (int id, Booking booking, IBookingService service) =>
        {
            try
            {
                var updated = await service.UpdateBookingAsync(id, booking);
                return updated != null ? Results.Ok(updated) : Results.NotFound();
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        });

        group.MapDelete("/{id:int}", async (int id, IBookingService service) =>
        {
            var canceled = await service.CancelBookingAsync(id);
            return canceled ? Results.NoContent() : Results.NotFound();
        });

        group.MapGet("/search", async (DateTime start, DateTime end, int guests, IBookingService service) =>
        {
            var rooms = await service.GetAvailableRoomsAsync(start, end, guests);
            return Results.Ok(rooms);
        });
    }

}