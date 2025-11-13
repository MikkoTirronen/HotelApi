using HotelApi.src.HotelApi.Core.Interfaces;
using HotelApi.src.HotelApi.Data.Interfaces;
using HotelApi.src.HotelApi.Domain.Entities;
using HotelApi.src.HotelApi.Domain.Enums;

namespace HotelApi.src.HotelApi.Core.Services
{
    public class PaymentService(IGenericRepository<PaymentRecord> paymentRepo, IGenericRepository<Invoice> invoiceRepo):IPaymentService
    {
        private readonly IGenericRepository<PaymentRecord> _paymentRepo = paymentRepo;
        private readonly IGenericRepository<Invoice> _invoiceRepo = invoiceRepo;

        public async Task<PaymentRecord> RegisterPaymentAsync(int invoiceId, decimal amount, string? method = null)
        {
            var invoice = await _invoiceRepo.GetByIdAsync(invoiceId) ?? throw new Exception("Invoice not found.");

            invoice.Payments ??= [];
            if (invoice.Payments.Count != 0)
                throw new Exception("Invoice has already been paid");

            var payment = new PaymentRecord
            {
                InvoiceId = invoice.Id,
                AmountPaid = amount,
                PaymentDate = DateTime.UtcNow,
                PaymentMethod = method
            };

            invoice.Status = InvoiceStatus.Paid;

            await _paymentRepo.AddAsync(payment);
            _invoiceRepo.Update(invoice);
            await _invoiceRepo.SaveAsync();

            return payment;
        }
    }
}