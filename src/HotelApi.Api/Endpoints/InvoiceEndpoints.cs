using HotelApi.src.HotelApi.Core.Interfaces;
using HotelApi.src.HotelApi.Domain.Entities;

namespace HotelApi.src.HotelApi.Api.Endpoints;

public static class InvoiceEndpoints
{
    public static void MapInvoiceEndpoints(this WebApplication app)
    {
        app.MapGet("/invoices", async (IInvoiceService service) =>
        {
            return Results.Ok(await service.GetAllInvoicesAsync());
        });

        app.MapGet("/invoices/{id:int}", async (int id, IInvoiceService service) =>
        {
            var invoice = await service.GetInvoiceByIdAsync(id);
            return invoice != null ? Results.Ok(invoice) : Results.NotFound();
        });

        app.MapGet("/invoices/booking/{bookingId:int}", async (int bookingId, IInvoiceService service) =>
        {
            var invoice = await service.GetInvoiceByBookingIdAsync(bookingId);
            return invoice != null ? Results.Ok(invoice) : Results.NotFound();
        });
    }
}
