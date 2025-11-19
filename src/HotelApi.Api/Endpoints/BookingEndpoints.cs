using HotelApi.src.HotelApi.Core.Interfaces;
using HotelApi.src.HotelApi.Domain.DTOs;
using HotelApi.src.HotelApi.Domain.Entities;

namespace HotelApi.src.HotelApi.Api.Endpoints;

public static class BookingEndPoints
{
    public static void MapBookingEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/bookings").WithTags("Bookings");

        group.MapGet("/", async (IBookingService service) =>
            Results.Ok(await service.GetAllBookingsAsync()));

        group.MapGet("/{id:int}", async (int id, IBookingService service) =>
        {
            var booking = await service.GetBookingByIdAsync(id);
            return booking != null ? Results.Ok(booking) : Results.NotFound();
        });
        group.MapGet("/search/existing", async (
                string? customer,
                string? room,
                int? bookingId,
                DateTime? startDate,
                DateTime? endDate,
                int? guests,
                IBookingService service) =>
        {
            var results = await service.AdvancedSearchAsync(
                customer,
                room,
                bookingId,
                startDate,
                endDate,
                guests);

            return Results.Ok(results);
        });

        group.MapPost("/", async (CreateBookingWithCustomerDto booking, IBookingService service) =>
        {
            try
            {
                var created = await service.CreateBookingAsync(booking);
                return Results.Created($"/{created?.BookingId}", created);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        });

        group.MapPut("/{id:int}", async (int id, UpdateBookingDto booking, IBookingService service) =>
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
            var canceled = await service.DeleteBookingAsync(id);
            return canceled ? Results.NoContent() : Results.NotFound();
        });

        group.MapGet("/search", async (DateTime start, DateTime end, int guests, IBookingService service) =>
        {
            var rooms = await service.GetAvailableRoomsAsync(start, end, guests);
            return Results.Ok(rooms);
        });
    }

}