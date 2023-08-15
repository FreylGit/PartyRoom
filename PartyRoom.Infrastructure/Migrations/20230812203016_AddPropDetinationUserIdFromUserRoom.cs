using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PartyRoom.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPropDetinationUserIdFromUserRoom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("778d4aae-6db3-46f1-a0b8-eecbc8a28977"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("bfb0ee32-f649-477e-9447-74239be5731d"));

            migrationBuilder.AddColumn<Guid>(
                name: "DestinationUserId",
                table: "UserRoom",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "FinishDate",
                table: "Rooms",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("4f70b23c-1e7f-4b2c-95d6-985f25949d46"), null, "Admin", "ADMIN" },
                    { new Guid("c3d52a0a-6813-407c-bfa1-671ad1e78f1d"), null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("4f70b23c-1e7f-4b2c-95d6-985f25949d46"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("c3d52a0a-6813-407c-bfa1-671ad1e78f1d"));

            migrationBuilder.DropColumn(
                name: "DestinationUserId",
                table: "UserRoom");

            migrationBuilder.DropColumn(
                name: "FinishDate",
                table: "Rooms");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("778d4aae-6db3-46f1-a0b8-eecbc8a28977"), null, "User", "USER" },
                    { new Guid("bfb0ee32-f649-477e-9447-74239be5731d"), null, "Admin", "ADMIN" }
                });
        }
    }
}
