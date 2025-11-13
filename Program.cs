using HotelApi.src.HotelApi.Core.Interfaces;
using HotelApi.src.HotelApi.Core.Services;
using HotelApi.src.HotelApi.Data.Contexts;
using HotelApi.src.HotelApi.Data.Interfaces;
using HotelApi.src.HotelApi.Data.Repos;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<HotelDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();

var app = builder.Build();

app.Run();