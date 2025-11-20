using HotelApi.src.HotelApi.Core.Interfaces;
using HotelApi.src.HotelApi.Data.Interfaces;
using HotelApi.src.HotelApi.Data.Repos;
using HotelApi.src.HotelApi.Domain.DTOs;
using HotelApi.src.HotelApi.Domain.Entities;
using HotelApi.src.HotelApi.Domain.Enums;

namespace HotelApi.src.HotelApi.Core.Services
{
    public class PaymentService(IGenericRepository<PaymentRecord> paymentRepo, IGenericRepository<Invoice> invoiceRepo, IPaymentRepository paymentRepository, IInvoiceRepository invoiceRepository) : IPaymentService
    {
        private readonly IGenericRepository<PaymentRecord> _paymentRepo = paymentRepo;
        private readonly IGenericRepository<Invoice> _invoiceRepo = invoiceRepo;
        private readonly IPaymentRepository _paymentRepository = paymentRepository;
        private readonly IInvoiceRepository _invoiceRepository = invoiceRepository;
        public async Task<PaymentDto> RegisterPaymentAsync(int invoiceId, string customer, decimal amount, string? method = null)
        {
            var invoice = await _invoiceRepository.GetInvoiceWithBookingAsync(invoiceId)
                ?? throw new Exception("Invoice not found.");

            if (invoice.Payments.Count != 0)
                throw new Exception("Invoice has already been paid");

            // Create payment
            var payment = new PaymentRecord
            {
                InvoiceId = invoice.InvoiceId,
                AmountPaid = amount,
                Customer = invoice.Booking.Customer.Name,
                PaymentDate = DateTime.UtcNow,
                PaymentMethod = method
            };

            // Update invoice and booking
            invoice.Status = InvoiceStatus.Paid;
            if (invoice.Booking != null)
                invoice.Booking.Status = BookingStatus.Confirmed;

            // Persist changes
            await _paymentRepo.AddAsync(payment);
            _invoiceRepo.Update(invoice);
            await _invoiceRepo.SaveAsync();
            await _paymentRepo.SaveAsync();

            return new PaymentDto(
                payment.PaymentId,
                payment.InvoiceId,
                payment.AmountPaid,
                payment.PaymentDate,
                payment.PaymentMethod,
                invoice.Booking?.Customer?.Name ?? "Unknown"
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