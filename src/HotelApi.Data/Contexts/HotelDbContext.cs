using HotelApi.src.HotelApi.Domain.Entities;
using HotelApi.src.HotelApi.Domain.Enums;
using Microsoft.EntityFrameworkCore;
namespace HotelApi.src.HotelApi.Data.Contexts;

public class HotelDbContext(DbContextOptions<HotelDbContext> options) : DbContext(options)
{
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<PaymentRecord> Payments => Set<PaymentRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Room>().HasIndex(r => r.RoomNumber).IsUnique();

        modelBuilder.Entity<Invoice>()
            .HasOne(i => i.Booking)
            .WithOne(b => b.Invoice)
            .HasForeignKey<Invoice>(i => i.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Customer)
            .WithMany(c => c.Bookings)
            .HasForeignKey(b => b.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Room)
            .WithMany(r => r.Bookings)
            .HasForeignKey(b => b.RoomId)
            .OnDelete(DeleteBehavior.Restrict);

        base.OnModelCreating(modelBuilder);

        // ----- Rooms -----
        modelBuilder.Entity<Room>().HasData(
            new Room { RoomId = 1, RoomNumber = "101", BaseCapacity = 1, PricePerNight = 80, MaxExtraBeds = 0 },
            new Room { RoomId = 2, RoomNumber = "102", BaseCapacity = 1, PricePerNight = 100, MaxExtraBeds = 1 },
            new Room { RoomId = 3, RoomNumber = "103", BaseCapacity = 1, PricePerNight = 100, MaxExtraBeds = 1 },
            new Room { RoomId = 4, RoomNumber = "201", BaseCapacity = 2, PricePerNight = 150, MaxExtraBeds = 0 },
            new Room { RoomId = 5, RoomNumber = "202", BaseCapacity = 2, PricePerNight = 150, MaxExtraBeds = 0 },
            new Room { RoomId = 6, RoomNumber = "203", BaseCapacity = 2, PricePerNight = 150, MaxExtraBeds = 2 },
            new Room { RoomId = 7, RoomNumber = "204", BaseCapacity = 2, PricePerNight = 150, MaxExtraBeds = 2 }
        );

        // ----- Customers (Looney Tunes) -----
        modelBuilder.Entity<Customer>().HasData(
            new Customer { CustomerId = 1, Name = "Bugs Bunny", Email = "bugs@looneytunes.com", Phone = "123-456" },
            new Customer { CustomerId = 2, Name = "Daffy Duck", Email = "daffy@looneytunes.com", Phone = "234-567" },
            new Customer { CustomerId = 3, Name = "Porky Pig", Email = "porky@looneytunes.com", Phone = "345-678" },
            new Customer { CustomerId = 4, Name = "Tweety Bird", Email = "tweety@looneytunes.com", Phone = "456-789" },
            new Customer { CustomerId = 5, Name = "Sylvester", Email = "sylvester@looneytunes.com", Phone = "567-890" }
        );

        // Common dates for the week-long stay
        var start = new DateTime(2025, 11, 17, 0, 0, 0, DateTimeKind.Utc);
        var end = new DateTime(2025, 11, 24, 0, 0, 0, DateTimeKind.Utc);

        // ----- Bookings -----
        modelBuilder.Entity<Booking>().HasData(
            new Booking
            {
                BookingId = 1,
                RoomId = 1,
                CustomerId = 1,
                StartDate = start,
                EndDate = end,
                NumPersons = 1,
                Status = BookingStatus.Confirmed,
                TotalPrice = 80 * 7,
                ExtraBedsCount = 0,
                CreatedAt = start,
                UpdatedAt = start,
                InvoiceId = 1
            },
            new Booking
            {
                BookingId = 2,
                RoomId = 2,
                CustomerId = 2,
                StartDate = start,
                EndDate = end,
                NumPersons = 1,
                Status = BookingStatus.Confirmed,
                TotalPrice = 100 * 7,
                ExtraBedsCount = 0,
                CreatedAt = start,
                UpdatedAt = start,
                InvoiceId = 2
            },
            new Booking
            {
                BookingId = 3,
                RoomId = 3,
                CustomerId = 3,
                StartDate = start,
                EndDate = end,
                NumPersons = 1,
                Status = BookingStatus.Confirmed,
                TotalPrice = 100 * 7,
                ExtraBedsCount = 0,
                CreatedAt = start,
                UpdatedAt = start,
                InvoiceId = 3
            },
            new Booking
            {
                BookingId = 4,
                RoomId = 4,
                CustomerId = 4,
                StartDate = start,
                EndDate = end,
                NumPersons = 2,
                Status = BookingStatus.Confirmed,
                TotalPrice = 150 * 7,
                ExtraBedsCount = 0,
                CreatedAt = start,
                UpdatedAt = start,
                InvoiceId = 4
            },
            new Booking
            {
                BookingId = 5,
                RoomId = 5,
                CustomerId = 5,
                StartDate = start,
                EndDate = end,
                NumPersons = 2,
                Status = BookingStatus.Confirmed,
                TotalPrice = 150 * 7,
                ExtraBedsCount = 0,
                CreatedAt = start,
                UpdatedAt = start,
                InvoiceId = 5
            }
        );

        // ----- Invoices -----
        modelBuilder.Entity<Invoice>().HasData(
            new Invoice
            {
                InvoiceId = 1,
                BookingId = 1,
                AmountDue = 80 * 7,
                IssueDate = start,
                Status = InvoiceStatus.Unpaid
            },
            new Invoice
            {
                InvoiceId = 2,
                BookingId = 2,
                AmountDue = 100 * 7,
                IssueDate = start,
                Status = InvoiceStatus.Unpaid
            },
            new Invoice
            {
                InvoiceId = 3,
                BookingId = 3,
                AmountDue = 100 * 7,
                IssueDate = start,
                Status = InvoiceStatus.Unpaid
            },
            new Invoice
            {
                InvoiceId = 4,
                BookingId = 4,
                AmountDue = 150 * 7,
                IssueDate = start,
                Status = InvoiceStatus.Unpaid
            },
            new Invoice
            {
                InvoiceId = 5,
                BookingId = 5,
                AmountDue = 150 * 7,
                IssueDate = start,
                Status = InvoiceStatus.Unpaid
            }
        );

    }
}