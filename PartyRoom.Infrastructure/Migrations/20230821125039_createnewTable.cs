using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PartyRoom.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class createnewTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("35c7410a-375d-4e04-aeb0-9629636cb8d5"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7f3cfe17-5d28-410b-a73a-5705804923d7"));

            migrationBuilder.CreateTable(
                name: "UserDetails",
                columns: table => new
                {
                    ApplicationUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    About = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDetails", x => x.ApplicationUserId);
                    table.ForeignKey(
                        name: "FK_UserDetails_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("02ad3e85-9c6f-4a4e-9ae9-a25631bf8172"), null, "Admin", "ADMIN" },
                    { new Guid("b46759cf-b2f4-45c9-bb6d-079f731733d0"), null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserDetails");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("02ad3e85-9c6f-4a4e-9ae9-a25631bf8172"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("b46759cf-b2f4-45c9-bb6d-079f731733d0"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("35c7410a-375d-4e04-aeb0-9629636cb8d5"), null, "User", "USER" },
                    { new Guid("7f3cfe17-5d28-410b-a73a-5705804923d7"), null, "Admin", "ADMIN" }
                });
        }
    }
}
