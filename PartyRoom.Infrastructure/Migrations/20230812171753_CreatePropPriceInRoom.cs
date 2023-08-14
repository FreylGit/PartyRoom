using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PartyRoom.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreatePropPriceInRoom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5cda96e9-972c-4cd2-9c4f-c856020e522d"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("bfba4a4e-33fa-49dd-9065-b9ddce7c300a"));

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Rooms",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("778d4aae-6db3-46f1-a0b8-eecbc8a28977"), null, "User", "USER" },
                    { new Guid("bfb0ee32-f649-477e-9447-74239be5731d"), null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("778d4aae-6db3-46f1-a0b8-eecbc8a28977"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("bfb0ee32-f649-477e-9447-74239be5731d"));

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Rooms");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("5cda96e9-972c-4cd2-9c4f-c856020e522d"), null, "Admin", "ADMIN" },
                    { new Guid("bfba4a4e-33fa-49dd-9065-b9ddce7c300a"), null, "User", "USER" }
                });
        }
    }
}
