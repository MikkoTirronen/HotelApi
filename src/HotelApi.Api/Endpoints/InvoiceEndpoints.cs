using HotelApi.src.HotelApi.Core.Interfaces;
using HotelApi.src.HotelApi.Data.Contexts;
using HotelApi.src.HotelApi.Domain.DTOs;
using HotelApi.src.HotelApi.Domain.Entities;
using HotelApi.src.HotelApi.Domain.Enums;

namespace HotelApi.src.HotelApi.Api.Endpoints;

public static class InvoiceEndpoints
{
    public static void MapInvoiceEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/invoices").WithTags("Invoices");

        group.MapGet("/", async (IInvoiceService service) =>
        {
            return Results.Ok(await service.GetAllInvoiceListAsync());
        });

        group.MapGet("/{id:int}", async (int id, IInvoiceService service) =>
        {
            var invoice = await service.GetInvoiceByIdAsync(id);
            return invoice != null ? Results.Ok(invoice) : Results.NotFound();
        });

        group.MapGet("/booking/{bookingId:int}", async (int bookingId, IInvoiceService service) =>
        {
            var invoice = await service.GetInvoiceByBookingIdAsync(bookingId);
            return invoice != null ? Results.Ok(invoice) : Results.NotFound();
        });

        app.MapPut("/invoices", async (
            InvoiceListDto dto,
            IInvoiceService invoiceService) =>
        {
            var updated = await invoiceService.UpdateInvoiceAsync(dto.InvoiceId, dto);

            return updated is null
                ? Results.NotFound("Invoice not found.")
                : Results.Ok(updated);
        });

        app.MapGet("/invoices/search", async (
            int? customerId,
            InvoiceStatus? status,
            string? customerName,
            IInvoiceService invoiceService
        ) =>
        {
            var results = await invoiceService.SearchInvoicesAsync(
            customerId,
            status,
            customerName
            );

            return Results.Ok(results);
        });
    }
}
