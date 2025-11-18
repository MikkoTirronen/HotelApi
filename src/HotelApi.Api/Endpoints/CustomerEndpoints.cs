using HotelApi.src.HotelApi.Core.Interfaces;
using HotelApi.src.HotelApi.Domain.DTOs;

public static class CustomerEndpoints
{
    public static void MapCustomerEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/customers").WithTags("Customers");

        group.MapGet("/", async (ICustomerService service) =>
        {
            return Results.Ok(await service.GetAllAsync());
        });

        group.MapGet("/{id:int}", async (int id, ICustomerService service) =>
        {
            var customer = await service.GetByIdAsync(id);
            return customer != null ? Results.Ok(customer) : Results.NotFound();
        });

        group.MapPost("/", async (CreateCustomerDto dto, ICustomerService service) =>
        {
            var created = await service.CreateAsync(dto);
            return Results.Created($"/{created.Id}", created);
        });

        group.MapPut("/{id:int}", async (int id, UpdateCustomerDto dto, ICustomerService service) =>
        {
            var updated = await service.UpdateAsync(id, dto);
            return updated != null ? Results.Ok(updated) : Results.NotFound();
        });

        group.MapDelete("/{id:int}", async (int id, ICustomerService service) =>
        {
            var deleted = await service.DeleteAsync(id);
            return deleted ? Results.NoContent() : Results.NotFound();
        });
    }
}