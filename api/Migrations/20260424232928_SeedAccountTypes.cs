using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class SeedAccountTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountTypes",
                table: "AccountTypes");

            migrationBuilder.RenameTable(
                name: "AccountTypes",
                newName: "AccountType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountType",
                table: "AccountType",
                column: "Id");

            migrationBuilder.InsertData(
                table: "AccountType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Checking" },
                    { 2, "Savings" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountType",
                table: "AccountType");

            migrationBuilder.DeleteData(
                table: "AccountType",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AccountType",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.RenameTable(
                name: "AccountType",
                newName: "AccountTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountTypes",
                table: "AccountTypes",
                column: "Id");
        }
    }
}
