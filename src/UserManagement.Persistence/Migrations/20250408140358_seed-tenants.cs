using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UserManagement.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class seedtenants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Tenants",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Tenants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Tenants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Tenants",
                type: "datetime2",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Tenants",
                columns: new[] { "Id", "Code", "CreatedAt", "DeletedAt", "IsActive", "IsDeleted", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("5e95cac8-4c4c-4b22-b5e2-d5b0e6b7c9e2"), "SGC", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Saint George Church", null },
                    { new Guid("8e95cac8-4c4c-4b22-b5e2-d5b0e6b7c9e3"), "MED", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "Mediansta", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tenants",
                keyColumn: "Id",
                keyValue: new Guid("5e95cac8-4c4c-4b22-b5e2-d5b0e6b7c9e2"));

            migrationBuilder.DeleteData(
                table: "Tenants",
                keyColumn: "Id",
                keyValue: new Guid("8e95cac8-4c4c-4b22-b5e2-d5b0e6b7c9e3"));

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Tenants");
        }
    }
}
