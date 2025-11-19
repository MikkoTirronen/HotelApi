using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelApi.Migrations
{
    /// <inheritdoc />
    public partial class initialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 6);

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "EndDate", "NumPersons", "StartDate", "TotalPrice", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), 1, new DateTime(2025, 11, 17, 0, 0, 0, 0, DateTimeKind.Utc), 560m, new DateTime(2025, 11, 17, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "EndDate", "ExtraBedsCount", "NumPersons", "StartDate", "Status", "TotalPrice", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 17, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), 0, 1, new DateTime(2025, 11, 17, 0, 0, 0, 0, DateTimeKind.Utc), 1, 700m, new DateTime(2025, 11, 17, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 1,
                columns: new[] { "Email", "Name", "Phone" },
                values: new object[] { "bugs@looneytunes.com", "Bugs Bunny", "123-456" });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 2,
                columns: new[] { "Email", "Name", "Phone" },
                values: new object[] { "daffy@looneytunes.com", "Daffy Duck", "234-567" });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerId", "CreatedAt", "Email", "Name", "Phone", "UpdatedAt" },
                values: new object[,]
                {
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "porky@looneytunes.com", "Porky Pig", "345-678", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "tweety@looneytunes.com", "Tweety Bird", "456-789", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "sylvester@looneytunes.com", "Sylvester", "567-890", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.UpdateData(
                table: "Invoices",
                keyColumn: "InvoiceId",
                keyValue: 1,
                columns: new[] { "AmountDue", "IssueDate" },
                values: new object[] { 560m, new DateTime(2025, 11, 17, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Invoices",
                keyColumn: "InvoiceId",
                keyValue: 2,
                columns: new[] { "AmountDue", "IssueDate" },
                values: new object[] { 700m, new DateTime(2025, 11, 17, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "BookingId", "CreatedAt", "CustomerId", "EndDate", "ExtraBedsCount", "InvoiceId", "NumPersons", "RoomId", "StartDate", "Status", "TotalPrice", "UpdatedAt" },
                values: new object[,]
                {
                    { 3, new DateTime(2025, 11, 17, 0, 0, 0, 0, DateTimeKind.Utc), 3, new DateTime(2025, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), 0, 3, 1, 3, new DateTime(2025, 11, 17, 0, 0, 0, 0, DateTimeKind.Utc), 1, 700m, new DateTime(2025, 11, 17, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, new DateTime(2025, 11, 17, 0, 0, 0, 0, DateTimeKind.Utc), 4, new DateTime(2025, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), 0, 4, 2, 4, new DateTime(2025, 11, 17, 0, 0, 0, 0, DateTimeKind.Utc), 1, 1050m, new DateTime(2025, 11, 17, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, new DateTime(2025, 11, 17, 0, 0, 0, 0, DateTimeKind.Utc), 5, new DateTime(2025, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), 0, 5, 2, 5, new DateTime(2025, 11, 17, 0, 0, 0, 0, DateTimeKind.Utc), 1, 1050m, new DateTime(2025, 11, 17, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Invoices",
                columns: new[] { "InvoiceId", "AmountDue", "BookingId", "IssueDate", "Status" },
                values: new object[,]
                {
                    { 3, 700m, 3, new DateTime(2025, 11, 17, 0, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { 4, 1050m, 4, new DateTime(2025, 11, 17, 0, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { 5, 1050m, 5, new DateTime(2025, 11, 17, 0, 0, 0, 0, DateTimeKind.Utc), 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Invoices",
                keyColumn: "InvoiceId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Invoices",
                keyColumn: "InvoiceId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Invoices",
                keyColumn: "InvoiceId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 5);

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "EndDate", "NumPersons", "StartDate", "TotalPrice", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), 2, new DateTime(2024, 2, 4, 0, 0, 0, 0, DateTimeKind.Utc), 240m, new DateTime(2025, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "EndDate", "ExtraBedsCount", "NumPersons", "StartDate", "Status", "TotalPrice", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 3, 5, 0, 0, 0, 0, DateTimeKind.Utc), 1, 3, new DateTime(2024, 3, 2, 0, 0, 0, 0, DateTimeKind.Utc), 0, 500m, new DateTime(2025, 2, 6, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 1,
                columns: new[] { "Email", "Name", "Phone" },
                values: new object[] { "john@example.com", "John Doe", "111-222" });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 2,
                columns: new[] { "Email", "Name", "Phone" },
                values: new object[] { "sarah@example.com", "Sarah Connor", "333-444" });

            migrationBuilder.UpdateData(
                table: "Invoices",
                keyColumn: "InvoiceId",
                keyValue: 1,
                columns: new[] { "AmountDue", "IssueDate" },
                values: new object[] { 240m, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Invoices",
                keyColumn: "InvoiceId",
                keyValue: 2,
                columns: new[] { "AmountDue", "IssueDate" },
                values: new object[] { 500m, new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "PaymentId", "AmountPaid", "InvoiceId", "PaymentDate", "PaymentMethod" },
                values: new object[] { 1, 240m, 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Credit Card" });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "RoomId", "Active", "Amenities", "BaseCapacity", "MaxExtraBeds", "PricePerNight", "RoomNumber", "Type" },
                values: new object[] { 6, true, null, 2, 2, 150m, "203", 0 });
        }
    }
}
