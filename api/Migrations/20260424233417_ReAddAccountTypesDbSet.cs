using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class ReAddAccountTypesDbSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountType",
                table: "AccountType");

            migrationBuilder.RenameTable(
                name: "AccountType",
                newName: "AccountTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountTypes",
                table: "AccountTypes",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
