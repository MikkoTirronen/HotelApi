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
            new Room { RoomId = 5, RoomNumber = "202", BaseCapacity = 2, PricePerNight = 150, MaxExtraBeds = 1 },
            new Room { RoomId = 6, RoomNumber = "203", BaseCapacity = 2, PricePerNight = 150, MaxExtraBeds = 2 }

        );

        // ----- Customers -----
        modelBuilder.Entity<Customer>().HasData(
            new Customer { CustomerId = 1, Name = "John Doe", Email = "john@example.com", Phone = "111-222" },
            new Customer { CustomerId = 2, Name = "Sarah Connor", Email = "sarah@example.com", Phone = "333-444" }
        );

        // ----- Bookings (must reference valid Room + Customer) -----
        modelBuilder.Entity<Booking>().HasData(
            new Booking
            {
                BookingId = 1,
                RoomId = 1,
                CustomerId = 1,
                StartDate = new DateTime(2024, 2, 4, 0, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(2024, 2, 5, 0, 0, 0, DateTimeKind.Utc),
                NumPersons = 2,
                Status = BookingStatus.Confirmed,
                TotalPrice = 240,
                ExtraBedsCount = 0,
                CreatedAt = new DateTime(2025, 1, 11, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 1, 11, 0, 0, 0, DateTimeKind.Utc),
                InvoiceId = 1
            },
            new Booking
            {
                BookingId = 2,
                RoomId = 2,
                CustomerId = 2,
                StartDate = new DateTime(2024, 3, 2, 0, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(2024, 3, 5, 0, 0, 0, DateTimeKind.Utc),
                NumPersons = 3,
                Status = BookingStatus.Pending,
                TotalPrice = 500,
                ExtraBedsCount = 1,
                CreatedAt = new DateTime(2025, 2, 5, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 2, 6, 0, 0, 0, DateTimeKind.Utc),
                InvoiceId = 2
            }
        );

        // ----- Invoices (must reference BookingId) -----
        modelBuilder.Entity<Invoice>().HasData(
            new Invoice
            {
                InvoiceId = 1,
                BookingId = 1,
                AmountDue = 240,
                IssueDate = new DateTime(2024, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                Status = InvoiceStatus.Unpaid
            },
            new Invoice
            {
                InvoiceId = 2,
                BookingId = 2,
                AmountDue = 500,
                IssueDate = new DateTime(2024, 3, 1, 0, 0, 0, DateTimeKind.Utc),
                Status = InvoiceStatus.Unpaid
            }
        );

        // ----- Payments -----
        modelBuilder.Entity<PaymentRecord>().HasData(
            new PaymentRecord
            {
                PaymentId = 1,
                InvoiceId = 1,
                AmountPaid = 240m,
                PaymentDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                PaymentMethod = "Credit Card"
            }
        );
    }
}