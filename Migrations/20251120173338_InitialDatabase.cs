using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    RoomId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoomNumber = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    BaseCapacity = table.Column<int>(type: "integer", nullable: false),
                    MaxExtraBeds = table.Column<int>(type: "integer", nullable: false),
                    PricePerNight = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    Amenities = table.Column<string>(type: "text", nullable: true),
                    Active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.RoomId);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoomId = table.Column<int>(type: "integer", nullable: false),
                    CustomerId = table.Column<int>(type: "integer", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NumPersons = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    ExtraBedsCount = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    InvoiceId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.BookingId);
                    table.ForeignKey(
                        name: "FK_Bookings_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bookings_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    InvoiceId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BookingId = table.Column<int>(type: "integer", nullable: false),
                    AmountDue = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.InvoiceId);
                    table.ForeignKey(
                        name: "FK_Invoices_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "BookingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InvoiceId = table.Column<int>(type: "integer", nullable: false),
                    AmountPaid = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PaymentMethod = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_Payments_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "InvoiceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerId", "CreatedAt", "Email", "Name", "Phone", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "bugs@looneytunes.com", "Bugs Bunny", "123-456", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "daffy@looneytunes.com", "Daffy Duck", "234-567", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "porky@looneytunes.com", "Porky Pig", "345-678", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "tweety@looneytunes.com", "Tweety Bird", "456-789", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "sylvester@looneytunes.com", "Sylvester", "567-890", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "RoomId", "Active", "Amenities", "BaseCapacity", "MaxExtraBeds", "PricePerNight", "RoomNumber", "Type" },
                values: new object[,]
                {
                    { 1, true, null, 1, 0, 80m, "101", 0 },
                    { 2, true, null, 1, 1, 100m, "102", 0 },
                    { 3, true, null, 1, 1, 100m, "103", 0 },
                    { 4, true, null, 2, 0, 150m, "201", 0 },
                    { 5, true, null, 2, 0, 150m, "202", 0 },
                    { 6, true, null, 2, 2, 150m, "203", 0 },
                    { 7, true, null, 2, 2, 150m, "204", 0 }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "BookingId", "CreatedAt", "CustomerId", "EndDate", "ExtraBedsCount", "InvoiceId", "NumPersons", "RoomId", "StartDate", "Status", "TotalPrice", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 11, 17, 0, 0, 0, 0, DateTimeKind.Utc), 1, new DateTime(2025, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), 0, 1, 1, 1, new DateTime(2025, 11, 17, 0, 0, 0, 0, DateTimeKind.Utc), 1, 560m, new DateTime(2025, 11, 17, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, new DateTime(2025, 11, 17, 0, 0, 0, 0, DateTimeKind.Utc), 2, new DateTime(2025, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), 0, 2, 1, 2, new DateTime(2025, 11, 17, 0, 0, 0, 0, DateTimeKind.Utc), 1, 700m, new DateTime(2025, 11, 17, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, new DateTime(2025, 11, 17, 0, 0, 0, 0, DateTimeKind.Utc), 3, new DateTime(2025, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), 0, 3, 1, 3, new DateTime(2025, 11, 17, 0, 0, 0, 0, DateTimeKind.Utc), 1, 700m, new DateTime(2025, 11, 17, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, new DateTime(2025, 11, 17, 0, 0, 0, 0, DateTimeKind.Utc), 4, new DateTime(2025, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), 0, 4, 2, 4, new DateTime(2025, 11, 17, 0, 0, 0, 0, DateTimeKind.Utc), 1, 1050m, new DateTime(2025, 11, 17, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, new DateTime(2025, 11, 17, 0, 0, 0, 0, DateTimeKind.Utc), 5, new DateTime(2025, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), 0, 5, 2, 5, new DateTime(2025, 11, 17, 0, 0, 0, 0, DateTimeKind.Utc), 1, 1050m, new DateTime(2025, 11, 17, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Invoices",
                columns: new[] { "InvoiceId", "AmountDue", "BookingId", "DueDate", "IssueDate", "Status" },
                values: new object[,]
                {
                    { 1, 560m, 1, null, new DateTime(2025, 11, 17, 0, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { 2, 700m, 2, null, new DateTime(2025, 11, 17, 0, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { 3, 700m, 3, null, new DateTime(2025, 11, 17, 0, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { 4, 1050m, 4, null, new DateTime(2025, 11, 17, 0, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { 5, 1050m, 5, null, new DateTime(2025, 11, 17, 0, 0, 0, 0, DateTimeKind.Utc), 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CustomerId",
                table: "Bookings",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_RoomId",
                table: "Bookings",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_BookingId",
                table: "Invoices",
                column: "BookingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_InvoiceId",
                table: "Payments",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_RoomNumber",
                table: "Rooms",
                column: "RoomNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Rooms");
        }
    }
}
