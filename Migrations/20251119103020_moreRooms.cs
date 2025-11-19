using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelApi.Migrations
{
    /// <inheritdoc />
    public partial class moreRooms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 5,
                column: "MaxExtraBeds",
                value: 0);

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "RoomId", "Active", "Amenities", "BaseCapacity", "MaxExtraBeds", "PricePerNight", "RoomNumber", "Type" },
                values: new object[,]
                {
                    { 6, true, null, 2, 2, 150m, "203", 0 },
                    { 7, true, null, 2, 2, 150m, "204", 0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 7);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 5,
                column: "MaxExtraBeds",
                value: 1);
        }
    }
}
