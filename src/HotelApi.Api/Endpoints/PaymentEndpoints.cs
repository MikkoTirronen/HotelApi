using HotelApi.src.HotelApi.Core.Interfaces;
using HotelApi.src.HotelApi.Domain.DTOs;

namespace HotelApi.src.HotelApi.Api.Endpoints;

public static class PaymentEndpoints
{
    public static void MapPaymentEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/payments").WithTags("Payments");

        group.MapPost("/", async (
            RegisterPaymentDto request,
            IPaymentService paymentService
        ) =>
        {
            var payment = await paymentService.RegisterPaymentAsync(
                request.InvoiceId,
                request.Customer,
                request.Amount,
                request.Method
            );

            return Results.Ok(payment);
        });

        group.MapGet("/", async (IPaymentService service) =>
        {
            var payments = await service.GetAllPaymentsAsync();
            return Results.Ok(payments);
        });
    }
}