using HotelApi.src.HotelApi.Core.Interfaces;
using HotelApi.src.HotelApi.Domain.DTOs;

public static class CustomerEndpoints
{
    public static void MapCustomerEndpoints(this WebApplication app)
    {
        app.MapGet("/customers", async (ICustomerService service) =>
        {
            return Results.Ok(await service.GetAllAsync());
        });

        app.MapGet("/customers/{id:int}", async (int id, ICustomerService service) =>
        {
            var customer = await service.GetByIdAsync(id);
            return customer != null ? Results.Ok(customer) : Results.NotFound();
        });

        app.MapPost("/customers", async (CreateCustomerDto dto, ICustomerService service) =>
        {
            var created = await service.CreateAsync(dto);
            return Results.Created($"/customers/{created.Id}", created);
        });

        app.MapPut("/customers/{id:int}", async (int id, UpdateCustomerDto dto, ICustomerService service) =>
        {
            var updated = await service.UpdateAsync(id, dto);
            return updated != null ? Results.Ok(updated) : Results.NotFound();
        });

        app.MapDelete("/customers/{id:int}", async (int id, ICustomerService service) =>
        {
            var deleted = await service.DeleteAsync(id);
            return deleted ? Results.NoContent() : Results.NotFound();
        });
    }
}