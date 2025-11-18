using HotelApi.src.HotelApi.Domain.Entities;

namespace HotelApi.src.HotelApi.Data.Interfaces;

public interface IInvoiceRepository: IGenericRepository<Invoice>
{
    Task<IEnumerable<Invoice>> GetInvoicesWithBookingsAsync();
    Task<Invoice?> GetInvoiceWithBookingsByIdAsync(int id);
}