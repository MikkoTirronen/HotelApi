using HotelApi.src.HotelApi.Core.Interfaces;
using HotelApi.src.HotelApi.Data.Interfaces;
using HotelApi.src.HotelApi.Domain.DTOs;
using HotelApi.src.HotelApi.Domain.Entities;
using HotelApi.src.HotelApi.Domain.Enums;

namespace HotelApi.src.HotelApi.Core.Services
{
    public class PaymentService(IGenericRepository<PaymentRecord> paymentRepo, IGenericRepository<Invoice> invoiceRepo, IPaymentRepository paymentRepository) : IPaymentService
    {
        private readonly IGenericRepository<PaymentRecord> _paymentRepo = paymentRepo;
        private readonly IGenericRepository<Invoice> _invoiceRepo = invoiceRepo;
        private readonly IPaymentRepository _paymentRepository = paymentRepository;
        public async Task<PaymentDto> RegisterPaymentAsync(int invoiceId, string customer, decimal amount, string? method = null)
        {
            var invoice = await _invoiceRepo.GetByIdAsync(invoiceId) ?? throw new Exception("Invoice not found.");

            invoice.Payments ??= [];
            if (invoice.Payments.Count != 0)
                throw new Exception("Invoice has already been paid");

            var payment = new PaymentRecord
            {
                InvoiceId = invoice.InvoiceId,
                Customer = customer,
                AmountPaid = amount,
                PaymentDate = DateTime.UtcNow,
                PaymentMethod = method
            };

            invoice.Status = InvoiceStatus.Paid;

            await _paymentRepo.AddAsync(payment);
            _invoiceRepo.Update(invoice);
            await _invoiceRepo.SaveAsync();

            return new PaymentDto(
                PaymentId: payment.PaymentId,
                InvoiceId: payment.InvoiceId,
                AmountPaid: payment.AmountPaid,
                PaymentDate: payment.PaymentDate,
                PaymentMethod: payment.PaymentMethod,
                CustomerName: payment.Customer
            );
        }

        public async Task<IEnumerable<PaymentDto>> GetAllPaymentsAsync()
        {
            var list = await _paymentRepository.GetAllPaymentsOrderedAsync();

            return list.Select(p => new PaymentDto(
                p.PaymentId,
                p.InvoiceId,
                p.AmountPaid,
                p.PaymentDate,
                p.PaymentMethod,
                p.Invoice.Booking.Customer.Name
            ));
        }

    }
}