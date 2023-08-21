using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PartyRoom.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPropIsStartDateFronRoom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("4f70b23c-1e7f-4b2c-95d6-985f25949d46"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("c3d52a0a-6813-407c-bfa1-671ad1e78f1d"));

            migrationBuilder.AddColumn<bool>(
                name: "IsStarted",
                table: "Rooms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("67de806f-338c-418d-bbe1-563d64406704"), null, "Admin", "ADMIN" },
                    { new Guid("681aac16-9f08-4fca-90f2-5c099f23d30c"), null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("67de806f-338c-418d-bbe1-563d64406704"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("681aac16-9f08-4fca-90f2-5c099f23d30c"));

            migrationBuilder.DropColumn(
                name: "IsStarted",
                table: "Rooms");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("4f70b23c-1e7f-4b2c-95d6-985f25949d46"), null, "Admin", "ADMIN" },
                    { new Guid("c3d52a0a-6813-407c-bfa1-671ad1e78f1d"), null, "User", "USER" }
                });
        }
    }
}
