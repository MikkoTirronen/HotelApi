using HotelApi.src.HotelApi.Core.Interfaces;

namespace HotelApi.src.HotelApi.Api.Endpoints;

public static class PaymentEndpoints
{
    public static void MapPaymentEndpoints(this WebApplication app)
    {
        app.MapPost("/payments", async (
        int invoiceId,
        decimal amount,
        string? method,
        IPaymentService paymentService
    ) =>
    {
        var payment = await paymentService.RegisterPaymentAsync(invoiceId, amount, method);
        return Results.Ok(payment);
    });
    }
}